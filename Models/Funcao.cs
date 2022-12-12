using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Funcao
{
    public int IdFuncao { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; } = new List<Funcionario>();
}
