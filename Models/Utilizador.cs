using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models;

public partial class Utilizador
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Rua { get; set; } = null!;

    public int Porta { get; set; }

    [Display(Name = "Código-Postal")]
    public string CodigoPostal { get; set; } = null!;

    public string Telemovel { get; set; } = null!;

    public string? Nif { get; set; }

    [Display(Name = "Data de Nascimento")]
    public DateTime? DataNascimento { get; set; }

    public DateOnly? DataOnly
    {
        get
        {
            if (DataNascimento != null)
            {
                DateTime dt = (DateTime)DataNascimento;
                return DateOnly.FromDateTime(dt);
            }
            return null;
        }
    }

    public string? UtilizadorPath { get; set; }

    public virtual ICollection<Promocaofeira> Promocaofeiras { get; } = new List<Promocaofeira>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();

    public virtual ICollection<Vendum> Venda { get; } = new List<Vendum>();

    public virtual ICollection<Feira> IdFeiras { get; } = new List<Feira>();
}
