using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Produto
{
    public int IdProduto { get; set; }

    public int Stock { get; set; }

    public string Descricao { get; set; } = null!;

    public decimal Preco { get; set; }

    public decimal Iva { get; set; }

    public string? ImagemPath { get; set; }

    public int StandId { get; set; }

    public virtual Stand? Stand { get; set; } = null!;

    public virtual ICollection<VendaProduto> VendaProdutos { get; } = new List<VendaProduto>();
}
