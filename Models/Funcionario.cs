using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Funcionario
{
    public int IdFuncionario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Telemovel { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string? FuncionarioPath { get; set; }

    public int Funcao { get; set; }

    public virtual Funcao FuncaoNavigation { get; set; } = null!;
}
