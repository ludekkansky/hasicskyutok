using System.ComponentModel.DataAnnotations;

namespace hasicskyutok.ViewModel
{
    public class Vysledek
    {
        public int DruzstvoID { get; set; }
        public string DruzstvoNazev { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime? VyslednyCas {get;set;}
        public int StartovniCislo {get;set;}
        public string KategorieNazev {get;set;}
    }
}
