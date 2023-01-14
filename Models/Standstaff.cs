using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Standstaff
{
    [Display(Name = "Stand ID")]
    public int IdStand { get; set; }

    [Display(Name = "Staff's member email")]
    [ForeignKey("staff_email")]
    public string StaffEmail { get; set; } = null!;

    [Display(Name = "Stand")]
    public virtual Stand? IdStandNavigation { get; set; } = null!;
}
