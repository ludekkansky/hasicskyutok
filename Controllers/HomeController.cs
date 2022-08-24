using Microsoft.AspNetCore.Mvc;
using hasicskyutok.Models;

namespace hasicskyutok.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly hasicskyutokDbContext _dbContext;
    public HomeController(ILogger<HomeController> logger, hasicskyutokDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
     public IActionResult Index()
     {
        var druzstva = _dbContext.Druzstva.Select(s=>new ViewModel.Vysledek()
        {
           DruzstvoNazev = s.Nazev,
           StartovniCislo = s.StartovniCislo 
        });

         return View(druzstva);
     }
}
