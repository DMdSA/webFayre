using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class VendaProduto
{

    [Display(Name = "Id da venda")]
    public int VendaId { get; set; }

    [Display(Name = "Id do produto")]
    public int ProdutoId { get; set; }

    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Display(Name = "Quantidade")]
    public decimal? Quantidade { get; set; }

    //[ForeignKey("produto_id")]
    [Display(Name = "Produto")]
    public virtual Produto? Produto { get; set; } = null!;

    //[ForeignKey("venda_id")]
    [Display(Name = "Venda")]
    public virtual Vendum? Venda { get; set; } = null!;
}
