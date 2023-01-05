using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Produto
{
    public int IdProduto { get; set; }

    public int Stock { get; set; }

    [Display(Name = "Nome")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Display(Name = "IVA")]
    public decimal Iva { get; set; }

    public string? ImagemPath { get; set; }

    public int StandId { get; set; }

    // pq eq aq n pode ter foreign key?...
    public virtual Stand? Stand { get; set; } = null!;

    //[ForeignKey("produto_id")]
    public virtual ICollection<VendaProduto> VendaProdutos { get; } = new List<VendaProduto>();
}
