using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace hasicskyutok.ViewModel
{
    public class Vysledek
    {
        public int DruzstvoID { get; set; }
        public string DruzstvoNazev { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:HH:mm:ss.fff}", ApplyFormatInEditMode = true), DataType(DataType.Time)]
        public DateTime? VyslednyCas1 { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:HH:mm:ss.fff}", ApplyFormatInEditMode = true), DataType(DataType.Time)]
        public DateTime? VyslednyCas2 { get; set; }
        public bool NeplatnyPokus1 { get; set; }
        public bool NeplatnyPokus2 { get; set; }
        public int StartovniCislo { get; set; }
        public string KategorieNazev { get; set; }
    }
}
