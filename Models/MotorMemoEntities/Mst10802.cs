using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst10802
{
    public int VehOwnerId { get; set; }

    public string VehicleNo { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public string OwnerName { get; set; } = null!;

    public string? OwnerAddress { get; set; }

    public decimal? OwnerMobno { get; set; }

    public string? OwnerPanNo { get; set; }

    public virtual Mst108 VehicleNoNavigation { get; set; } = null!;
}
