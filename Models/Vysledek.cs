using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace hasicskyutok.Models;

public class Vysledek : IComparable
{
    public int ID { get; set; }
    [BindProperty, DisplayFormat(DataFormatString = "{0:HH:mm:ss.fff}", ApplyFormatInEditMode = true), DataType(DataType.Time)]
    public DateTime? Cas1 { get; set; }
    public bool NeplatnyPokus1 { get; set; }
    [BindProperty, DisplayFormat(DataFormatString = "{0:HH:mm:ss.fff}", ApplyFormatInEditMode = true), DataType(DataType.Time)]
    public DateTime? Cas2 { get; set; }
    public bool NeplatnyPokus2 { get; set; }
    public int DruzstvoID { get; set; }
    [ForeignKey(nameof(DruzstvoID))]
    public Druzstvo Druzstvo { get; set; }

    private static DateTime? VratNejlepsiCas(Vysledek vysledek)
    {
        if (!vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas1 == null && vysledek.Cas2 == null) return null;
        if (vysledek.Cas1 == null && vysledek.Cas2 == null) return null;
        if (vysledek.NeplatnyPokus1 && vysledek.NeplatnyPokus2) return null;

        if (vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas2 != null) return vysledek.Cas2;
        if (!vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas1 == null && vysledek.Cas2.HasValue) return vysledek.Cas2;

        if (!vysledek.NeplatnyPokus1 && vysledek.NeplatnyPokus2 && vysledek.Cas1 != null) return vysledek.Cas1;
        if (!vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas1 != null && vysledek.Cas2 != null)
        {
            if (vysledek.Cas1 < vysledek.Cas2)
                return vysledek.Cas1;
            else
                return vysledek.Cas2;
        }
        if (!vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas1 != null && vysledek.Cas2 == null) return vysledek.Cas1;

        throw new Exception($"Chybi podminka na {vysledek.Cas1}{vysledek.NeplatnyPokus1} {vysledek.Cas2}{vysledek.NeplatnyPokus2}");
    }

    public int CompareTo(object obj)
    {
        var vysledek = obj as Vysledek;
        DateTime? vysledekA = VratNejlepsiCas(this);
        Console.WriteLine("Porovnáváme:--------------------------------");
        Console.WriteLine($"Družstvo:{Druzstvo.Nazev} T1:{Cas1:HH:mm:ss} Neplatný:{NeplatnyPokus1} - T2:{Cas2:HH:mm:ss} Neplatný:{NeplatnyPokus2} vysledek={vysledekA}");
        DateTime? vysledekB = VratNejlepsiCas(vysledek);
        Console.WriteLine($"Druzstvo:{vysledek.Druzstvo.Nazev} {vysledek.Cas1:HH:mm:ss} Neplatny:{vysledek.NeplatnyPokus1} - {vysledek.Cas2:HH:mm:ss} Neplatny:{vysledek.NeplatnyPokus2} vysledek={vysledekB}");
        Console.WriteLine("--------------------------------------------");

        if (vysledekA == null && vysledekB != null)
        {
            Console.WriteLine("Výsledek 1 není, druhý je vracíme 1");
            return 1;
        }
        if (vysledekA == null && vysledekB == null)
        {
            Console.WriteLine("Výsledek 1 není, druhý taky ne vracíme 0");
            return 0;
        }
        if (vysledekA != null && vysledekB == null)
        {
            Console.WriteLine("Výsledek 1 máme, druhý není vracíme -1");
            return -1;
        }

        if (vysledekA.Value.TimeOfDay < vysledekB.Value.TimeOfDay)
        {
            Console.WriteLine("A je lepší vracíme -1");
            return -1;
        }
        if (vysledekA == vysledekB)
        {
            Console.WriteLine("Jsou stejné vraícme 0");
            return 0;
        }
        if (vysledekA.Value.TimeOfDay > vysledekB.Value.TimeOfDay)
        {
            Console.WriteLine("B je lepší vracíme 1");
            return 1;
        }

        return 0;
    }
}
