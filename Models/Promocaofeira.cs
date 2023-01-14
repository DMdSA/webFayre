using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Promocaofeira
{
    [Display(Name = "Id")]
    public int IdPromocaoFeira { get; set; }

    [Display(Name = "Max Capacity")]
    public int CapacidadeUtilizadores { get; set; }

    [Display(Name = "Description")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Name")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Number of stands")]
    public int NStands { get; set; }

    [Display(Name = "User id")]
    public int IdUtilizador { get; set; }

    [Display(Name = "Staff id")]
    public int? IdFuncionario { get; set; }

    [Display(Name = "Staff member")]
    public virtual Funcionario? IdFuncionarioNavigation { get; set; }

    [Display(Name = "User")]
    public virtual Utilizador? IdUtilizadorNavigation { get; set; } = null!;
}
