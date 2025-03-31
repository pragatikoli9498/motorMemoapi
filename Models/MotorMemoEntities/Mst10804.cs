using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst10804
{
    public string VehicleNo { get; set; } = null!;

    public string? BankName { get; set; }

    public string? Address { get; set; }

    public string? BankaccNo { get; set; }

    public string? IfscCode { get; set; }

    public virtual Mst108 VehicleNoNavigation { get; set; } = null!;
}
