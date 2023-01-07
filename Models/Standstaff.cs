using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFayre.Models;

public partial class Standstaff
{
    [Display(Name = "Stand")]
    public int IdStand { get; set; }

    [Display(Name = "Staff")]
    [ForeignKey("staff_email")]
    public string StaffEmail { get; set; } = null!;

    public virtual Stand? IdStandNavigation { get; set; } = null!;
}
