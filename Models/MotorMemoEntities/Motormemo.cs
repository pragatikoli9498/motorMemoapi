using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Motormemo
{
    public int VchId { get; set; }

    public int? FirmId { get; set; }

    public string? DivId { get; set; } = null!;

    public DateTime? Dt { get; set; }

    public string? From_Dstn { get; set; }

    public string? To_Dstn { get; set; } 

    public string VehicleNo { get; set; } = null!;

    public int? MemoNo { get; set; }

    public int? LeftAmount { get; set; }

    public decimal? TotalFreight { get; set; }
     
    public decimal? AdvAmount { get; set; }

    public int? FreightType { get; set; }

    public int? BillAmt { get; set; }

    public DateTime? ConfDate { get; set; }

    public int? KiloMiter { get; set; }

    public int? vehAccCode { get; set; }

    public virtual MotormemoAudit? MotormemoAudit { get; set; }

    public virtual MotormemoDetail? MotormemoDetails { get; set; }

    public virtual ICollection<MotormemoCommodity> MotormemoCommodities { get; set; } = new List<MotormemoCommodity>();
      
    public virtual ICollection<MotormemoExpense> MotormemoExpenses { get; set; } = new List<MotormemoExpense>();

    public virtual ICollection<MotormemoOtherCharges> MotormemoOtherCharges { get; set; } = new List<MotormemoOtherCharges>();

    public virtual ICollection<Acc003> Acc003s { get; set; } = new List<Acc003>();

    public virtual Mst011? VehicleAccNavigation { get; set; }

    public virtual ICollection<Tms011_01> Tms01101s { get; set; } = new List<Tms011_01>();
}
