using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HasicskyUtok.Models;

public class Druzstvo
{
    public int ID { get; set; }
    [Display(Name="Název")]
    public string Nazev { get; set; }
    [Display(Name="Startovní číslo")]
    public int StartovniCislo { get; set; }
    [Display(Name="Kategorie")]
    public int KategorieID { get; set; }
    [ForeignKey(nameof(KategorieID))]
    public Kategorie Kategorie { get; set; }
    public int StafetaID { get; set; }
    [ForeignKey(nameof(StafetaID))]
    public Stafeta Stafeta { get; set; }
}