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

    public int CompareTo(object obj)
    {
        var vysledek = obj as Vysledek;
        DateTime? lepsiCasA = new DateTime();
        DateTime? lepsiCasB = new DateTime();
        if (!NeplatnyPokus1) lepsiCasA = Cas1;
        if (lepsiCasA == DateTime.MinValue && !NeplatnyPokus2) lepsiCasA = Cas2;
        if (!NeplatnyPokus2 && lepsiCasA > Cas2) lepsiCasA = Cas2;

        if (!vysledek.NeplatnyPokus1) lepsiCasB = vysledek.Cas1;
        if (lepsiCasB == DateTime.MinValue && !vysledek.NeplatnyPokus2) lepsiCasB = vysledek.Cas2;
        if (!vysledek.NeplatnyPokus2 && lepsiCasB > vysledek.Cas2) lepsiCasB = vysledek.Cas2;

        if (lepsiCasA == DateTime.MinValue && lepsiCasB != DateTime.MinValue) return 1;
        if (lepsiCasA == DateTime.MinValue && lepsiCasB == DateTime.MinValue) return 0;
        if (lepsiCasA != DateTime.MinValue && lepsiCasB == DateTime.MinValue) return -1;

        //min value je bohuzel z yroy, ale z db prijde i null
        if (lepsiCasA == null && lepsiCasB != DateTime.MinValue) return 1;
        if (lepsiCasA == null && lepsiCasB == null) return 0;
        if (lepsiCasA != null && lepsiCasB == null) return -1;

        if (lepsiCasA < lepsiCasB) return -1;
        if (lepsiCasA == lepsiCasB) return 0;
        if (lepsiCasA > lepsiCasB) return 1;

        return 0;
    }
}