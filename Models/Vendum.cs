using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Vendum
{
    [Display(Name = "Código de venda")]
    public int IdVenda { get; set; }

    [Display(Name = "Data")]
    public DateTime Data { get; set; }

    [Display(Name = "Total")]
    public decimal Total { get; set; }[ForeignKey("venda_id")]

    [Display(Name = "Regateio")]
    public decimal? ValorRegateio { get; set; }

    [Display(Name = "Id do cliente")]
    public int UtilizadorId { get; set; }

    [Display(Name = "Id do stand")]
    public int StandId { get; set; }


    //[ForeignKey("stand_id")]
    public virtual Stand? Stand { get; set; } = null!;
    
    //[ForeignKey("utilizador_id")]
    public virtual Utilizador? Utilizador { get; set; } = null!;

    //[ForeignKey("venda_id")]
    public virtual ICollection<VendaProduto> VendaProdutos { get; set; } = new List<VendaProduto>();
}
