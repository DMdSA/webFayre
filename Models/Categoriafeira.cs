using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Categoriafeira
{
    [Display(Name = "ID")]
    public int IdCategoriaFeira { get; set; }

    public string Descricao { get; set; } = null!;

    [ForeignKey("feira_id")]
    public virtual ICollection<Feira> Feiras { get; } = new List<Feira>();
}
