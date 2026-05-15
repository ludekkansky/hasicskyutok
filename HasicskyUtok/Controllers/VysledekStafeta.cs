using Microsoft.AspNetCore.Mvc;
using HasicskyUtok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HasicskyUtok.Controllers;

public class VysledekStafetaController : Controller
{
    private readonly ILogger<VysledekStafetaController> _logger;
    private readonly HasicskyUtokDbContext _dbContext;
    private readonly IHubContext<ChatHub> _chatHub;

    public VysledekStafetaController(ILogger<VysledekStafetaController> logger, HasicskyUtokDbContext dbContext, IHubContext<ChatHub> chatHub)
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
                       join vysledek in _dbContext.VysledkyStafeta on druzstvo.ID equals vysledek.DruzstvoID
                       select (new ViewModel.Vysledek()
                       {
                           DruzstvoID = druzstvo.ID,
                           DruzstvoNazev = druzstvo.Nazev,
                           StartovniCislo = druzstvo.StartovniCislo,
                           VyslednyCas1 = vysledek.CasStafeta1,
                           VyslednyCas2 = vysledek.CasStafeta2,
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
            return RedirectToAction("Detail", "VysledekStafeta", new { id = druzstvo.ID });
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

        var vysledek = _dbContext.VysledkyStafeta.FirstOrDefault(s => s.DruzstvoID == druzstvo.ID);
        if (vysledek == null)
        {
            vysledek = new Models.VysledekStafeta()
            {
                DruzstvoID = druzstvo.ID
            };
            _dbContext.VysledkyStafeta.Add(vysledek);
            _dbContext.SaveChanges();
        }

        var druzstvoVysledek = new ViewModel.DruzstvoVysledek()
        {
            ID = druzstvo.ID,
            DruzstvoNazev = druzstvo.Nazev,
            StartovniCislo = druzstvo.StartovniCislo,
            Vysledek1 = vysledek.CasStafeta1?.ToString("HH:mm:ss.fff"),
            Vysledek2 = vysledek.CasStafeta2?.ToString("HH:mm:ss.fff"),
            NeplatnyPokus1 = vysledek.NeplatnyPokus1,
            NeplatnyPokus2 = vysledek.NeplatnyPokus2
        };

        return View(druzstvoVysledek);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Detail(ViewModel.DruzstvoVysledek vysledek)
    {
        var parse1 = TimeOnly.TryParse(vysledek.Vysledek1, out TimeOnly cas1);
        var parse2 = TimeOnly.TryParse(vysledek.Vysledek2, out TimeOnly cas2);

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
        var vysledekDB = _dbContext.VysledkyStafeta.FirstOrDefault(s => s.DruzstvoID == vysledek.ID);
        if (vysledekDB == null)
        {
            vysledekDB = new Models.VysledekStafeta()
            {
                DruzstvoID = vysledek.ID,
                CasStafeta1 = cas1 == TimeOnly.MinValue ? null : cas1,
                CasStafeta2 = cas2 == TimeOnly.MinValue ? null : cas2,
                NeplatnyPokus1 = vysledek.NeplatnyPokus1,
                NeplatnyPokus2 = vysledek.NeplatnyPokus2
            };
            _dbContext.VysledkyStafeta.Add(vysledekDB);
        }
        else
        {
            vysledekDB.CasStafeta1 = cas1 == TimeOnly.MinValue ? null : cas1;
            vysledekDB.CasStafeta2 = cas2 == TimeOnly.MinValue ? null : cas2;
            vysledekDB.NeplatnyPokus1 = vysledek.NeplatnyPokus1;
            vysledekDB.NeplatnyPokus2 = vysledek.NeplatnyPokus2;
        }
        await _dbContext.SaveChangesAsync();

        var druzstvo = await _dbContext.Druzstva.FirstOrDefaultAsync(s => s.ID == vysledekDB.ID);
        await _chatHub.Clients.All.SendAsync("UpdateVysledek", vysledekDB.ID, druzstvo.StartovniCislo, druzstvo.Nazev, vysledekDB.CasStafeta1, vysledekDB.CasStafeta2);

        return RedirectToAction("Index", "Home");
    }
}
