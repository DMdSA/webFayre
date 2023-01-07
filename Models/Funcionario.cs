using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Funcionario
{
    public int IdFuncionario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Display(Name = "Telemóvel")]
    public string Telemovel { get; set; } = null!;

    [Display(Name = "Data de criação")]
    public DateTime CreationDate { get; set; }

    public DateOnly? DataOnly
    {
        get
        {
            DateTime dt = (DateTime)CreationDate;
            return DateOnly.FromDateTime(dt);
        }
    }

    [Display(Name = "Imagem de perfil")]
    public string? FuncionarioPath { get; set; }

    [ForeignKey("funcao_funcionario")]
    public int Funcao { get; set; }
    [Display(Name = "Função")]
    public virtual Funcao? FuncaoNavigation { get; set; } = null!;

    public virtual ICollection<Promocaofeira> Promocaofeiras { get; } = new List<Promocaofeira>();
}
