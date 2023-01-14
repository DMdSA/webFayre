using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class StandPatrocinador
{
    [Display(Name = "ID")]
    public int IdStand { get; set; }

    [Display(Name = "Sponsor ID")]
    public int IdPatrocinador { get; set; }

    [Display(Name = "Sponsor")]
    public virtual Patrocinador IdPatrocinadorNavigation { get; set; } = null!;
}
