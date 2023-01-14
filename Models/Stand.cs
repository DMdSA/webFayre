using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Stand
{
    [Display(Name = "ID")]
    public int IdStand { get; set; }

    [Display(Name = "Description")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Name")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Contact email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Contact phone")]
    public string Telefone { get; set; } = null!;

    [Display(Name = "Availability")]
    public short Disponibilidade { get; set; }

    [Display(Name = "Address")]
    public string? Morada { get; set; }

    [Display(Name = "Fair ID")]
    [ForeignKey("stand_feira_id")]
    public int FeiraId { get; set; }

    [Display(Name = "Image path")]
    public string? StandPath { get; set; }

    [Display(Name = "Stand's type ID")]
    [ForeignKey("stand_tipo_id")]
    public int StandTipoId { get; set; }

    [Display(Name = "Fair")]
    public virtual Feira? Feira { get; set; } = null!;

    [Display(Name = "Products")]
    public virtual ICollection<Produto> Produtos { get; } = new List<Produto>();

    [Display(Name = "Stand Type")]
    public virtual TipoStand? StandTipo { get; set; } = null!;

    [Display(Name = "Stand's staff crew")]
    public virtual ICollection<Standstaff> Standstaffs { get; } = new List<Standstaff>();

    [Display(Name = "Sales")]
    public virtual ICollection<Vendum> Venda { get; } = new List<Vendum>();

    [Display(Name = "Sponsors")]
    public virtual ICollection<Patrocinador> IdPatrocinadors { get; } = new List<Patrocinador>();
}
