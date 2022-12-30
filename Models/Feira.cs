using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Feira
{
    [Display(Name = "Id")]
    public int IdFeira { get; set; }

    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = null!;

    public string Nome { get; set; } = null!;

    [Display(Name = "Data-Início")]
    public DateTime DataInicio { get; set; }

    [Display(Name = "Data-Fim")]
    public DateTime DataFim { get; set; }

    [Display(Name = "Capacidade de clientes")]
    public int CapacidadeClientes { get; set; }

    [Display(Name = "Número de stands")]
    public int NStands { get; set; }

    public string Email { get; set; } = null!;

    public string? Telefone { get; set; }

    public string? Morada { get; set; }

    [Display(Name = "Img Path")]
    public string? FeiraPath { get; set; }

    public virtual ICollection<Stand> Stands { get; set; } = new List<Stand>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [ForeignKey("feira_categoria")]
    public virtual ICollection<Categoriafeira> FeiraCategoria1s { get; set; } = new List<Categoriafeira>();

    [ForeignKey("id_utilizador")]
    public virtual ICollection<Utilizador> IdUtilizadors { get; set; } = new List<Utilizador>();

    public virtual ICollection<Patrocinador> Patrocinadors { get; set; } = new List<Patrocinador>();
}
