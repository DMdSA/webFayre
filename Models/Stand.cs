using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Stand
{
    public int IdStand { get; set; }

    public string Descricao { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public short Disponibilidade { get; set; }

    public string? Morada { get; set; }

    [ForeignKey("stand_feira_id")]
    public int FeiraId { get; set; }

    public string? StandPath { get; set; }

    [ForeignKey("stand_tipo_id")]
    public int StandTipoId { get; set; }

    public virtual Feira? Feira { get; set; } = null!;

    public virtual ICollection<Produto> Produtos { get; } = new List<Produto>();

    public virtual TipoStand? StandTipo { get; set; } = null!;

    public virtual ICollection<Standstaff> Standstaffs { get; } = new List<Standstaff>();

    public virtual ICollection<Patrocinador> IdPatrocinadors { get; } = new List<Patrocinador>();
}
