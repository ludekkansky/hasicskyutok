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

    private DateTime? Dejmi(Vysledek vysledek)
    {
        /*
Druzstvo:Páté družstvo 04.09.2022 0:00:17False-False vysledek=04.09.2022 0:00:17
Druzstvo:Druhé družstvo 04.09.2022 0:00:12False-True vysledek=04.09.2022 0:00:12
Druzstvo:Třetí družstvo 18.09.2022 0:00:12False-False vysledek=18.09.2022 0:00:12
Druzstvo:Páté družstvo 04.09.2022 0:00:17False-False vysledek=04.09.2022 0:00:17
Druzstvo:Čtvrté družstvo 18.09.2022 0:00:15False-False vysledek=18.09.2022 0:00:15
Druzstvo:Třetí družstvo 18.09.2022 0:00:12False-False vysledek=18.09.2022 0:00:12
Druzstvo:Šesté družstvo 18.09.2022 0:00:18False-False vysledek=18.09.2022 0:00:18
Druzstvo:Čtvrté družstvo 18.09.2022 0:00:15False-False vysledek=18.09.2022 0:00:15
        */
        if (vysledek.NeplatnyPokus1 && vysledek.NeplatnyPokus2) return null;
        if (vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas2 != null) return vysledek.Cas2;
        if (!vysledek.NeplatnyPokus1 && vysledek.NeplatnyPokus2 && vysledek.Cas1 != null) return vysledek.Cas1;
        if (!vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas1 != null && vysledek.Cas2 != null) return vysledek.Cas1;
        if (!vysledek.NeplatnyPokus1 && !vysledek.NeplatnyPokus2 && vysledek.Cas1 != null && vysledek.Cas2 == null) return vysledek.Cas1;

        throw new Exception($"Chybi podminka na {vysledek.Cas1}{vysledek.NeplatnyPokus1} {vysledek.Cas2}{vysledek.NeplatnyPokus2}");
    }

    public int CompareTo(object obj)
    {
        var vysledek = obj as Vysledek;
        DateTime? vysledekA = Dejmi(this);
        Console.WriteLine($"Druzstvo:{this.Druzstvo.Nazev} {this.Cas1}{this.NeplatnyPokus1}-{this.Cas2}{this.NeplatnyPokus2} vysledek={vysledekA}");
        DateTime? vysledekB = Dejmi(vysledek);
        Console.WriteLine($"Druzstvo:{vysledek.Druzstvo.Nazev} {vysledek.Cas1}{vysledek.NeplatnyPokus1}-{vysledek.Cas2}{vysledek.NeplatnyPokus2} vysledek={vysledekB}");

        if (vysledekA == null && vysledekB != null) return 1;
        if (vysledekA == null && vysledekB == null) return 0;
        if (vysledekA != null && vysledekB == null) return -1;

        if (vysledekA.Value.TimeOfDay < vysledekB.Value.TimeOfDay) return -1;
        if (vysledekA == vysledekB) return 0;
        if (vysledekA.Value.TimeOfDay > vysledekB.Value.TimeOfDay) return 1;

        return 0;
    }
}