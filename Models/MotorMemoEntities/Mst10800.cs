using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst10800
{
    public string VehicleNo { get; set; } = null!;

    public string? Enginno { get; set; }

    public string? Chassisno { get; set; }

    public string? DriverAddress { get; set; }

    public string? DriverName { get; set; }

    public decimal DriverMobileNo { get; set; }

    public int? DriverLicNo { get; set; }
     
}
