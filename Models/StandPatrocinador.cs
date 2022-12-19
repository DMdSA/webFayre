using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class StandPatrocinador
{
    public int IdStand { get; set; }

    public int IdPatrocinador { get; set; }

    public virtual Patrocinador IdPatrocinadorNavigation { get; set; } = null!;
}
