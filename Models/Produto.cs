using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Produto
{
    [Display(Name = "ID")]
    public int IdProduto { get; set; }

    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [Display(Name = "Name")]
    public string Name { get; set; } = null!;

    [Display(Name = "Description")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Price")]
    public decimal Preco { get; set; }

    [Display(Name = "IVA")]
    public decimal Iva { get; set; }

    [Display(Name = "Image path")]
    public string? ImagemPath { get; set; }

    [Display(Name = "Stand ID")]
    public int StandId { get; set; }

    [Display(Name = "Stand")]
    public virtual Stand? Stand { get; set; } = null!;

    [Display(Name = "Sales")]
    public virtual ICollection<VendaProduto> VendaProdutos { get; } = new List<VendaProduto>();
}
