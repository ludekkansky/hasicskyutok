using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HasicskyUtok.ViewModel;

public class VysledekFinale
{
    public string Nazev { get; internal set; }

    [
        BindProperty,
        DisplayFormat(DataFormatString = "{0:HH:mm:ss.fff}", ApplyFormatInEditMode = true),
        DataType(DataType.Time)
    ]
    public TimeOnly? Cas { get; internal set; }
    public object Kategorie { get; internal set; }
}
