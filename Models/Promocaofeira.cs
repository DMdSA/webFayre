using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Promocaofeira
{
    public int IdPromocaoFeira { get; set; }

    public int CapacidadeUtilizadores { get; set; }

    public string Descricao { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public int NStands { get; set; }

    public int IdUtilizador { get; set; }

    public int? IdFuncionario { get; set; }

    public virtual Funcionario? IdFuncionarioNavigation { get; set; }

    public virtual Utilizador? IdUtilizadorNavigation { get; set; } = null!;
}
