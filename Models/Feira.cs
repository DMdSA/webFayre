using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Feira
{
    [Display(Name = "Id")]
    public int IdFeira { get; set; }

    [Display(Name = "Description")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Name")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Beginning date")]
    public DateTime DataInicio { get; set; }

    [Display(Name = "End date")]
    public DateTime DataFim { get; set; }

    [Display(Name = "Clients Capacity")]
    public int CapacidadeClientes { get; set; }

    [Display(Name = "Number of stands")]
    public int NStands { get; set; }

    [Display(Name = "Contact email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Contact phone")]
    public string? Telefone { get; set; }

    [Display(Name = "Address")]
    public string? Morada { get; set; }

    [Display(Name = "Image path")]
    public string? FeiraPath { get; set; }

    [Display(Name = "Base price")]
    public decimal PrecoBase { get; set; }

    [Display(Name = "Stands")]
    public virtual ICollection<Stand> Stands { get; set; } = new List<Stand>();

    [Display(Name = "Tickets")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [Display(Name = "Categories")]
    [ForeignKey("feira_categoria")]
    public virtual ICollection<Categoriafeira> FeiraCategoria1s { get; set; } = new List<Categoriafeira>();

    [Display(Name = "Users")]
    [ForeignKey("id_utilizador")]
    public virtual ICollection<Utilizador> IdUtilizadors { get; set; } = new List<Utilizador>();

    [Display(Name = "Sponsors")]
    public virtual ICollection<Patrocinador> Patrocinadors { get; set; } = new List<Patrocinador>();
}
