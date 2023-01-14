using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class TipoStand
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Description")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Stands")]
    public virtual ICollection<Stand> Stands { get; } = new List<Stand>();
}
