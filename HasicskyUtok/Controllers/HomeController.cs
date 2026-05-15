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

    public HomeController(ILogger<HomeController> logger, HasicskyUtokDbContext dbContext, IHubContext<ChatHub> chatHub)
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
        var vysledky = _dbContext.VysledkyUtok.Where(s => s.NeplatnyPokus1 == false || s.NeplatnyPokus2 == false).OrderBy(s => s.Cas1).ThenBy(s => s.Cas2).Include(s => s.Druzstvo);
        return View(vysledky);
    }
    public IActionResult VysledkyStafeta()
    {
        var vysledky = _dbContext.VysledkyStafeta.Where(s => s.NeplatnyPokus1 == false || s.NeplatnyPokus2 == false).OrderBy(s => s.CasStafeta1).ThenBy(s => s.CasStafeta2).Include(s => s.Druzstvo);
        return View(vysledky);
    }
}
