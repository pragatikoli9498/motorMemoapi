using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst10803
{
    public string VehicleNo { get; set; } = null!;

    public int FirmId { get; set; }

    public int ProvAccCode { get; set; }

    public int ExpAccCode { get; set; }

    public decimal? BranchId { get; set; }

    public virtual Mst011? ExpAccCodeNavigation { get; set; }

    public virtual Mst011? ProvAccCodeNavigation { get; set; }

    public virtual Mst108? VehicleNoNavigation { get; set; }
}
