using System.ComponentModel.DataAnnotations.Schema;

namespace hasicskyutok.Models;

public class Druzstvo
{
    public int ID { get; set; }
    public string Nazev { get; set; }
    public int StartovniCislo { get; set; }
    public int KategorieID { get; set; }
    [ForeignKey(nameof(KategorieID))]
    public Kategorie Kategorie { get; set; }
}