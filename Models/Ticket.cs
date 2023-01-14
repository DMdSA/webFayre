using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Ticket
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Date")]
    public DateTime Data { get; set; }

    [Display(Name = "User ID")]
    [ForeignKey("FK_ticket_utilizador")]
    public int UtilizadorId { get; set; }

    [Display(Name = "Fair ID")]
    [ForeignKey("FK_ticket_feira")]
    public int FeiraId { get; set; }

    [Display(Name = "Fair")]
    public virtual Feira Feira { get; set; } = null!;

    [Display(Name = "User")]
    public virtual Utilizador Utilizador { get; set; } = null!;
}
