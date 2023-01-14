using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Utilizador
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Display(Name = "Street")]
    public string Rua { get; set; } = null!;

    [Display(Name = "Door number")]
    public int Porta { get; set; }

    [Display(Name = "Zip-code")]
    public string CodigoPostal { get; set; } = null!;

    [Display(Name = "Phone")]
    public string Telemovel { get; set; } = null!;

    [Display(Name = "NIF")]
    public string? Nif { get; set; }

    [Display(Name = "Birthday")]
    public DateTime? DataNascimento { get; set; }

    [Display(Name = "Birthday")]
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

    [Display(Name = "Image path")]
    public string? UtilizadorPath { get; set; }

    [Display(Name = "Fair promotions requests")]
    public virtual ICollection<Promocaofeira> Promocaofeiras { get; set; } = new List<Promocaofeira>();

    [Display(Name = "Tickets")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [Display(Name = "Purchases")]
    public virtual ICollection<Vendum> Venda { get; set; } = new List<Vendum>();

    [Display(Name = "Favorite fairs")]
    [ForeignKey("id_feira")]
    public virtual ICollection<Feira> IdFeiras { get; set; } = new List<Feira>();
}
