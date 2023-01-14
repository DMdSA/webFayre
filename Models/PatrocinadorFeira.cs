using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class PatrocinadorFeira
{
    [Display(Name = "Fair ID")]
    public int FeiraId { get; set; }

    [Display(Name = "Sponsor ID")]
    public int PatrocinadorId { get; set; }

    [Display(Name = "Sponsor")]
    public virtual Patrocinador Patrocinador { get; set; } = null!;
}
