using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Patrocinador
{

    [Display(Name = "ID")]
    public int IdPatrocinador { get; set; }

    [Display(Name = "Name")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Contact email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Info")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Contact phone")]
    public string? Telefone { get; set; }

    [Display(Name = "Sponsored fairs")]
    public virtual ICollection<Feira> Feiras { get; } = new List<Feira>();

    [Display(Name = "Sponsored stands")]
    public virtual ICollection<Stand> IdStands { get; } = new List<Stand>();
}
