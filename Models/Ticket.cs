using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public DateTime Data { get; set; }

    public int UtilizadorId { get; set; }

    public string FeiraId { get; set; } = null!;

    public virtual Utilizador Utilizador { get; set; } = null!;
}
