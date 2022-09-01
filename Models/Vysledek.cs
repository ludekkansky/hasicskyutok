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
        if (this.NeplatnyPokus1 == false && this.Cas1 < vysledek.Cas1 && vysledek.NeplatnyPokus1 == false) return -1;
        if (this.NeplatnyPokus1 == false && this.Cas1 < vysledek.Cas2 && vysledek.NeplatnyPokus2 == false) return -1;
        if (this.NeplatnyPokus2 == false && this.Cas2 < vysledek.Cas2 && vysledek.NeplatnyPokus2 == false) return -1;
        return 1;
    }

}