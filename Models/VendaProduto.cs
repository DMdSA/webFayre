using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class VendaProduto
{
    [Display(Name = "ID")]
    public int VendaId { get; set; }

    [Display(Name = "Product ID")]
    public int ProdutoId { get; set; }

    [Display(Name = "Final price")]
    public decimal Preco { get; set; }

    [Display(Name = "Quantity")]
    public decimal? Quantidade { get; set; }

    [Display(Name = "Product")]
    public virtual Produto? Produto { get; set; } = null!;

    [Display(Name = "Sales")]
    public virtual Vendum? Venda { get; set; } = null!;
}
