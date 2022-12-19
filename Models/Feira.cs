using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Feira
{
    public int IdFeira { get; set; }

    public string Descricao { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public int CapacidadeClientes { get; set; }

    public int NStands { get; set; }

    public string Email { get; set; } = null!;

    public string? Telefone { get; set; }

    public string? Morada { get; set; }

    public string? FeiraPath { get; set; }

    public virtual ICollection<Stand> Stands { get; } = new List<Stand>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();

    public virtual ICollection<Categoriafeira> FeiraCategoria1s { get; } = new List<Categoriafeira>();

    public virtual ICollection<Utilizador> IdUtilizadors { get; } = new List<Utilizador>();

    public virtual ICollection<Patrocinador> Patrocinadors { get; } = new List<Patrocinador>();
}
