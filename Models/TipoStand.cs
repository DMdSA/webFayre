using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class TipoStand
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Stand> Stands { get; } = new List<Stand>();
}
