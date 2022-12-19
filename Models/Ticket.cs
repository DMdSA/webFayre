using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public DateTime Data { get; set; }

    [ForeignKey("FK_ticket_utilizador")]
    public int UtilizadorId { get; set; }

    [ForeignKey("FK_ticket_feira")]
    public int FeiraId { get; set; }

    public virtual Feira Feira { get; set; } = null!;

    public virtual Utilizador Utilizador { get; set; } = null!;
}
