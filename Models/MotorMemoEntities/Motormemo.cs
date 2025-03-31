using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Motormemo
{
    public int VchId { get; set; }

    public int? FirmId { get; set; }

    public string? DivId { get; set; } = null!;

    public string? Dt { get; set; }

    public string? From { get; set; }

    public string? To { get; set; } 

    public string VehicleNo { get; set; } = null!;

    public int? MemoNo { get; set; }

    public int? ownerCreditAmout { get; set; }

    public decimal? freightdeductAmount { get; set; }
     
    public decimal? advanceAmount { get; set; }

    public int? selectfreightType { get; set; }

    public int? buildtotalamt { get; set; }

    public virtual MotormemoAudit? MotormemoAudit { get; set; }

    public virtual MotormemoDetail? MotormemoDetails { get; set; }

    public virtual ICollection<MotormemoCommodity> MotormemoCommodities { get; set; } = new List<MotormemoCommodity>();
      
    public virtual ICollection<MotormemoExpense> MotormemoExpenses { get; set; } = new List<MotormemoExpense>();

    public virtual ICollection<MotormemoOtherCharges> MotormemoOtherCharges { get; set; } = new List<MotormemoOtherCharges>();

    public virtual ICollection<Acc003> Acc003s { get; set; } = new List<Acc003>();
}
