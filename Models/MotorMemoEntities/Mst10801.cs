using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst10801
{
    public int VehTaxId { get; set; }

    public string VehicleNo { get; set; } = null!;

    public decimal? InsuranceNo { get; set; }

    public string? InsuranceFrom { get; set; }

    public string? InsuranceTo { get; set; }

    public decimal? FitnessNo { get; set; }

    public string? FitnessFrom { get; set; }

    public string? FitnessTo { get; set; }

    public decimal? Permitno { get; set; }

    public string? PermitFm { get; set; }

    public string? PermitTo { get; set; }

    //public virtual Mst00603 State { get; set; } = null!;

    public virtual Mst108 VehicleNoNavigation { get; set; } = null!;
}
