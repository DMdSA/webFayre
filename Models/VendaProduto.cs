using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class VendaProduto
{
    public int VendaId { get; set; }

    public int ProdutoId { get; set; }

    public decimal Preco { get; set; }

    public decimal? Quantidade { get; set; }

    public virtual Produto Produto { get; set; } = null!;

    public virtual Vendum Venda { get; set; } = null!;
}
