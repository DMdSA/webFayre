using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class PatrocinadorFeira
{
    public int FeiraId { get; set; }

    public int PatrocinadorId { get; set; }

    public virtual Patrocinador Patrocinador { get; set; } = null!;
}
