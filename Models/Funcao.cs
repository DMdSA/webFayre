using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Funcao
{
    [Display(Name = "ID")]
    public int IdFuncao { get; set; }

    [Display(Name = "Description")]
    public string Descricao { get; set; } = null!;

    [Display(Name = "Staff crew")]
    public virtual ICollection<Funcionario> Funcionarios { get; } = new List<Funcionario>();
}
