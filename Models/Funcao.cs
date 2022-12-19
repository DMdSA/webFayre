using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Funcao
{
    [Display(Name = "ID")]
    public int IdFuncao { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; } = new List<Funcionario>();
}
