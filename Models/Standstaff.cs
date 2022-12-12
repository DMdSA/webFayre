using System;
using System.Collections.Generic;

namespace WebFayre.Models;

public partial class Standstaff
{
    public int IdStand { get; set; }

    public string StaffEmail { get; set; } = null!;

    public virtual Stand IdStandNavigation { get; set; } = null!;
}
