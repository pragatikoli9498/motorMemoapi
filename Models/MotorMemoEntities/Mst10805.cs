using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst10805
{
    public int VehSId { get; set; }

    public string VehicleNo { get; set; } = null!;

    public int StateId { get; set; }

    public string? Taxno { get; set; }

    public string? TaxFm { get; set; }

    public string? TaxTo { get; set; }

    public string? Permitno { get; set; }

    public string? PermitFm { get; set; }

    public string? PermitTo { get; set; }

    public virtual Mst00603 State { get; set; } = null!;

    public virtual Mst108 VehicleNoNavigation { get; set; } = null!;
}
