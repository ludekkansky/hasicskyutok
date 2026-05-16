using Microsoft.AspNetCore.Mvc;
using HasicskyUtok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HasicskyUtok.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HasicskyUtokDbContext _dbContext;
    private readonly IHubContext<ChatHub> _chatHub;

    public HomeController(
        ILogger<HomeController> logger,
        HasicskyUtokDbContext dbContext,
        IHubContext<ChatHub> chatHub
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        _chatHub = chatHub;
    }

    public IActionResult Index()
    {
        /*
        var druzstva = GetDruzstva();

        _chatHub.Clients.All.SendAsync("ReceiveMessage", "user", "Getting index");
        */
        return View();
    }

    public IActionResult VysledkyUtok()
    {
        var vysledky = _dbContext.VysledkyUtok
            .Where(s => s.NeplatnyPokus1 == false || s.NeplatnyPokus2 == false)
            .OrderBy(s => s.Cas1)
            .ThenBy(s => s.Cas2)
            .Include(s => s.Druzstvo);
        return View(vysledky);
    }

    public IActionResult VysledkyStafeta()
    {
        var vysledky = _dbContext.VysledkyStafeta
            .Where(s => s.NeplatnyPokus1 == false || s.NeplatnyPokus2 == false)
            .OrderBy(s => s.CasStafeta1)
            .ThenBy(s => s.CasStafeta2)
            .Include(s => s.Druzstvo);
        return View(vysledky);
    }

    public IActionResult VysledkyFinale()
    {
        var finaloveStafetyIDs = _dbContext.Stafeta
            .Where(s => s.DoFinale == true)
            .Select(s => s.ID)
            .ToList();

        var druzstvaFinaleIDs = _dbContext.Druzstva
            .Where(d => finaloveStafetyIDs.Contains(d.StafetaID))
            .Select(d => d.ID)
            .ToList();

        var vysledkyStafetaList = _dbContext.VysledkyStafeta
            .Where(
                s =>
                    druzstvaFinaleIDs.Contains(s.DruzstvoID)
                    && (s.NeplatnyPokus1 == false || s.NeplatnyPokus2 == false)
            )
            .OrderBy(s => s.CasStafeta1)
            .ThenBy(s => s.CasStafeta2)
            .Include(s => s.Druzstvo)
            .ThenInclude(d => d.Kategorie)
            .ToList();

        var finaloveStafeta = vysledkyStafetaList
            .GroupBy(s => s.DruzstvoID)
            .Select(g =>
            {
                var druzstvo = g.First().Druzstvo;
                var casy = g.Select(s => HasicskyUtok.Models.VysledekStafeta.VratNejlepsiCas(s))
                    .Where(t => t.HasValue)
                    .Select(t => t.Value.ToTimeSpan())
                    .ToList();
                return new
                {
                    DruzstvoID = g.Key,
                    druzstvo.Nazev,
                    Kategorie = druzstvo.Kategorie.Nazev,
                    CasStafeta = casy.Count > 0 ? (TimeSpan?)casy.Min() : null
                };
            })
            .ToList();

        var vysledkyUtokList = _dbContext.VysledkyUtok
            .Where(
                s =>
                    druzstvaFinaleIDs.Contains(s.DruzstvoID)
                    && (s.NeplatnyPokus1 == false || s.NeplatnyPokus2 == false)
            )
            .OrderBy(s => s.Cas1)
            .ThenBy(s => s.Cas2)
            .Include(s => s.Druzstvo)
            .ThenInclude(d => d.Kategorie)
            .ToList();

        var finaloveUtok = vysledkyUtokList
            .GroupBy(s => s.DruzstvoID)
            .Select(g =>
            {
                var druzstvo = g.First().Druzstvo;
                var casy = g.Select(s => HasicskyUtok.Models.VysledekUtok.VratNejlepsiCas(s))
                    .Where(t => t.HasValue)
                    .Select(t => t.Value.ToTimeSpan())
                    .ToList();
                return new
                {
                    DruzstvoID = g.Key,
                    druzstvo.Nazev,
                    Kategorie = druzstvo.Kategorie.Nazev,
                    CasUtok = casy.Count > 0 ? (TimeSpan?)casy.Min() : null
                };
            })
            .ToList();

        var allDruzstvoIDs =
         finaloveStafeta
            .Select(x => x.DruzstvoID)
            .Union(finaloveUtok.Select(x => x.DruzstvoID));

            //var spojene = allDruzstvoIDs.GroupBy(s=>s).Where(s=>s.Count()>1).Select(s=>s.Key).ToList();

        var finaloveVysledky =allDruzstvoIDs
            .Select(id =>
            {
                var stafeta = finaloveStafeta.FirstOrDefault(x => x.DruzstvoID == id);
                var utok = finaloveUtok.FirstOrDefault(x => x.DruzstvoID == id);

                var nazev = stafeta?.Nazev ?? utok?.Nazev;
                var kategorie = stafeta?.Kategorie ?? utok?.Kategorie;
                var minStafeta = stafeta?.CasStafeta;
                var minUtok = utok?.CasUtok;

                TimeOnly? cas = null;
                if (minStafeta.HasValue && minUtok.HasValue)
                {
                    cas = TimeOnly.FromTimeSpan(minStafeta.Value + minUtok.Value);
                }
                else if (minStafeta.HasValue)
                {
                    cas = TimeOnly.FromTimeSpan(minStafeta.Value);
                }
                else if (minUtok.HasValue)
                {
                    cas = TimeOnly.FromTimeSpan(minUtok.Value);
                }

                return new ViewModel.VysledekFinale
                {
                    Nazev = nazev,
                    Kategorie = kategorie,
                    Cas = cas,
                    Utok = !minUtok.HasValue
                };
            })
            .ToList();

        return View(finaloveVysledky);
    }
}
