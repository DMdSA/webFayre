using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Categoriafeira
{
    [Display(Name = "ID")]
    public int IdCategoriaFeira { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Feira> Feiras { get; } = new List<Feira>();
}
