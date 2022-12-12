using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Vendum
{
    public string IdVenda { get; set; } = null!;

    public DateTime Data { get; set; }

    public decimal Total { get; set; }

    public decimal? ValorRegateio { get; set; }

    public int UtilizadorId { get; set; }

    public string StandId { get; set; } = null!;

    public virtual Utilizador Utilizador { get; set; } = null!;

    public virtual ICollection<VendaProduto> VendaProdutos { get; } = new List<VendaProduto>();
}
