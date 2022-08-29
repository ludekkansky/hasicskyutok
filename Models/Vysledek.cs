using System.ComponentModel.DataAnnotations.Schema;

namespace hasicskyutok.Models;

public class Vysledek
{
    public int ID { get; set; }
    public TimeOnly? Cas1 { get; set; }
    public TimeOnly? Cas2 { get; set; }
    public int DruzstvoID { get; set; }
    [ForeignKey(nameof(DruzstvoID))]
    public Druzstvo Druzstvo { get; set; }
}