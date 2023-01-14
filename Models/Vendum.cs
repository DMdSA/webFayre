using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Vendum
{
    [Display(Name = "ID")]
    public int IdVenda { get; set; }

    [Display(Name = "Date")]
    public DateTime Data { get; set; }

    [Display(Name = "Total")]
    public decimal Total { get; set; }

    [Display(Name = "Bargain price")]
    public decimal? ValorRegateio { get; set; }

    [Display(Name = "User ID")]
    public int UtilizadorId { get; set; }

    [Display(Name = "Stand ID")]
    public int StandId { get; set; }

    [Display(Name = "NIF")]
    public string Nif { get; set; } = null!;

    [Display(Name = "Phone")]
    public string Telemovel { get; set; } = null!;

    [Display(Name = "Stand")]
    public virtual Stand? Stand { get; set; } = null!;

    [Display(Name = "User")]
    public virtual Utilizador? Utilizador { get; set; } = null!;

    [Display(Name = "Cart products")]
    public virtual ICollection<VendaProduto> VendaProdutos { get; set; } = new List<VendaProduto>();
}
