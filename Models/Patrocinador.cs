using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Patrocinador
{
    public int IdPatrocinador { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public string? Telefone { get; set; }

    public virtual ICollection<Feira> Feiras { get; } = new List<Feira>();

    public virtual ICollection<Stand> IdStands { get; } = new List<Stand>();
}
