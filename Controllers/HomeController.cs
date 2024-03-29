﻿using Microsoft.AspNetCore.Mvc;
using hasicskyutok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace hasicskyutok.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly hasicskyutokDbContext _dbContext;
    private readonly IHubContext<ChatHub> _chatHub;

    public HomeController(ILogger<HomeController> logger, hasicskyutokDbContext dbContext, IHubContext<ChatHub> chatHub)
    {
        _logger = logger;
        _dbContext = dbContext;
        _chatHub = chatHub;
    }
    public IActionResult Index()
    {
        var druzstva = GetDruzstva();

        _chatHub.Clients.All.SendAsync("ReceiveMessage", "user", "Getting index");
        return View(druzstva);
    }

    private IQueryable GetDruzstva()
    {
        var druzstva = from druzstvo in _dbContext.Druzstva
                       join vysledek in _dbContext.Vysledky on druzstvo.ID equals vysledek.DruzstvoID
                       select (new ViewModel.Vysledek()
                       {
                           DruzstvoID = druzstvo.ID,
                           DruzstvoNazev = druzstvo.Nazev,
                           StartovniCislo = druzstvo.StartovniCislo,
                           VyslednyCas1 = vysledek.Cas1,
                           VyslednyCas2 = vysledek.Cas2,
                           KategorieNazev = druzstvo.Kategorie.Nazev,
                           NeplatnyPokus1 = vysledek.NeplatnyPokus1,
                           NeplatnyPokus2 = vysledek.NeplatnyPokus2
                       });
        return druzstva;
    }

    public IActionResult Vyhledat(int? startovniCislo)
    {
        if (!startovniCislo.HasValue)
        {
            ViewBag.Chyba = $"Zadejte startovní číslo.";
            return View("Index", GetDruzstva());
        }

        var druzstvo = _dbContext.Druzstva.FirstOrDefault(s => s.StartovniCislo == startovniCislo.Value);
        if (druzstvo != null)
        {
            return RedirectToAction("Detail", "Home", new { id = druzstvo.ID });
        }
        else
        {
            ViewBag.Chyba = $"Startovní číslo {startovniCislo} nenalezeno";
            return View("Index", GetDruzstva());
        }
    }

    [HttpGet]
    public IActionResult Detail(int id)
    {
        var druzstvo = _dbContext.Druzstva.FirstOrDefault(s => s.ID == id);
        if (druzstvo == null)
            return NotFound("Druzstvo nenalezeno.");

        var vysledek = _dbContext.Vysledky.FirstOrDefault(s => s.DruzstvoID == druzstvo.ID);
        if (vysledek == null)
        {
            vysledek = new Models.Vysledek()
            {
                DruzstvoID = druzstvo.ID
            };
            _dbContext.Vysledky.Add(vysledek);
            _dbContext.SaveChanges();
        }

        var druzstvoVysledek = new ViewModel.DruzstvoVysledek()
        {
            ID = druzstvo.ID,
            DruzstvoNazev = druzstvo.Nazev,
            StartovniCislo = druzstvo.StartovniCislo,
            Vysledek1 = vysledek.Cas1?.ToString("HH:mm:ss.fff"),
            Vysledek2 = vysledek.Cas2?.ToString("HH:mm:ss.fff"),
            NeplatnyPokus1 = vysledek.NeplatnyPokus1,
            NeplatnyPokus2 = vysledek.NeplatnyPokus2
        };

        return View(druzstvoVysledek);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Detail(ViewModel.DruzstvoVysledek vysledek)
    {
        var parse1 = DateTime.TryParse(vysledek.Vysledek1, out DateTime cas1);
        var parse2 = DateTime.TryParse(vysledek.Vysledek2, out DateTime cas2);

        if (!string.IsNullOrEmpty(vysledek.Vysledek1) && parse1 == false)
        {
            ModelState.AddModelError("Vysledek1", "Čas není ve správném formátu.");
            return View(vysledek);
        }

        if (!string.IsNullOrEmpty(vysledek.Vysledek2) && parse2 == false)
        {
            ModelState.AddModelError("Vysledek2", "Čas není ve správném formátu.");
            return View(vysledek);
        }

        //tady ulozime vysledek
        var vysledekDB = _dbContext.Vysledky.FirstOrDefault(s => s.DruzstvoID == vysledek.ID);
        if (vysledekDB == null)
        {
            vysledekDB = new Models.Vysledek()
            {
                DruzstvoID = vysledek.ID,
                Cas1 = cas1 == DateTime.MinValue ? null : cas1,
                Cas2 = cas2 == DateTime.MinValue ? null : cas2,
                NeplatnyPokus1 = vysledek.NeplatnyPokus1,
                NeplatnyPokus2 = vysledek.NeplatnyPokus2
            };
            _dbContext.Vysledky.Add(vysledekDB);
        }
        else
        {
            vysledekDB.Cas1 = cas1 == DateTime.MinValue ? null : cas1;
            vysledekDB.Cas2 = cas2 == DateTime.MinValue ? null : cas2;
            vysledekDB.NeplatnyPokus1 = vysledek.NeplatnyPokus1;
            vysledekDB.NeplatnyPokus2 = vysledek.NeplatnyPokus2;
        }
        await _dbContext.SaveChangesAsync();

        var druzstvo = await _dbContext.Druzstva.FirstOrDefaultAsync(s => s.ID == vysledekDB.ID);
        await _chatHub.Clients.All.SendAsync("UpdateVysledek", vysledekDB.ID, druzstvo.StartovniCislo, druzstvo.Nazev, vysledekDB.Cas1, vysledekDB.Cas2);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Vysledky()
    {
        var vysledky = _dbContext.Vysledky.Where(s => s.NeplatnyPokus1 == false || s.NeplatnyPokus2 == false).OrderBy(s => s.Cas1).ThenBy(s => s.Cas2).Include(s => s.Druzstvo);

        return View(vysledky);
    }

    public IActionResult Test()
    {
        return View();
    }
}
