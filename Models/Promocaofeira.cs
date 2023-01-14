using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Promocaofeira
{
    [Display(Name = "ID")]
    public int IdPromocaoFeira { get; set; }

    [Display(Name = "Max Capacity")]
    public int CapacidadeUtilizadores { get; set; }

    [Display(Name = "Description")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Name")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Number of stands")]
    public int NStands { get; set; }

    [Display(Name = "isValidated")]
    public byte? IsValidado { get; set; }

    [Display(Name = "isValidated")]
    public Boolean isValidated
    {
        get
        {
            if (this.IsValidado == 0) return false;
            return true;
        }
    }


    [Display(Name = "User ID")]
    public int IdUtilizador { get; set; }

    [Display(Name = "Staff ID")]
    public int? IdFuncionario { get; set; }

    [Display(Name = "Staff member")]
    public virtual Funcionario? IdFuncionarioNavigation { get; set; }

    [Display(Name = "User")]
    public virtual Utilizador? IdUtilizadorNavigation { get; set; } = null!;
}
