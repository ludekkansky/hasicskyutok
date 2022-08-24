using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hasicskyutok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

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
     public async Task<IActionResult> Index()
     {
        //_dbContext.Kategorie.Add(new Kategorie(){Nazev="Muži"});
        //_dbContext.Kategorie.Add(new Kategorie(){Nazev="Ženy"});
        //await _dbContext.SaveChangesAsync();
        //_dbContext.Druzstva.Add(new Druzstvo(){Nazev="Test",StartovniCislo=1,KategorieID=1});
        //await _dbContext.SaveChangesAsync();
        var druzstva = _dbContext.Druzstva.Select(s=>new ViewModel.Vysledek()
        {
           DruzstvoNazev = s.Nazev,
           StartovniCislo = s.StartovniCislo 
        });

         return View(druzstva);
     }
}
