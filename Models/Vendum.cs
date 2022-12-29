using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Vendum
{
    public int IdVenda { get; set; }

    public DateTime Data { get; set; }

    public decimal Total { get; set; }

    public decimal? ValorRegateio { get; set; }

    public int UtilizadorId { get; set; }

    public int StandId { get; set; }

    public virtual Stand Stand { get; set; } = null!;

    public virtual Utilizador Utilizador { get; set; } = null!;

    public virtual ICollection<VendaProduto> VendaProdutos { get; } = new List<VendaProduto>();
}
