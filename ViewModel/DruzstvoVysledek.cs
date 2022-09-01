using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace hasicskyutok.ViewModel;

public class DruzstvoVysledek
{
    public int ID { get; set; }
    public string DruzstvoNazev { get; set; }
    public int StartovniCislo { get; set; }
    public string Vysledek1 { get; set; }
    public string Vysledek2 { get; set; }
    public bool NeplatnyPokus1 { get; set; }
    public bool NeplatnyPokus2 { get; set; }
}