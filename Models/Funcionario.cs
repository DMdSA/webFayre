using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Funcionario
{
    [Display(Name = "ID")]
    public int IdFuncionario { get; set; }

    [Display(Name = "Name")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Display(Name = "Phone")]
    public string Telemovel { get; set; } = null!;

    [Display(Name = "Creation date")]
    public DateTime CreationDate { get; set; }

    [Display(Name = "Date")]
    public DateOnly? DataOnly
    {
        get
        {
            DateTime dt = (DateTime)CreationDate;
            return DateOnly.FromDateTime(dt);
        }
    }

    [Display(Name = "Image path")]
    public string? FuncionarioPath { get; set; }

    [Display(Name = "Role ID")]
    [ForeignKey("funcao_funcionario")]
    public int Funcao { get; set; }

    [Display(Name = "Role")]
    public virtual Funcao? FuncaoNavigation { get; set; } = null!;

    [Display(Name = "Validated fair promotions")]
    public virtual ICollection<Promocaofeira> Promocaofeiras { get; } = new List<Promocaofeira>();
}
