using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Categoriafeira
{
    public int IdCategoriaFeira { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Feira> Feiras { get; } = new List<Feira>();
}
