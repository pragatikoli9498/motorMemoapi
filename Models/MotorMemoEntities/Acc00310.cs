using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00310
{
    public int VchId { get; set; }

    public string? SupplierGstin { get; set; }

    public int SupplierType { get; set; }

    public string? ToGstin { get; set; }

    public string? PartyName { get; set; }

    public int PartyType { get; set; }

    public string StateType { get; set; } = null!;

    public int StateCode { get; set; }

    public string? Sec7act { get; set; }

    public string? Ecommgstn { get; set; }

    public virtual Acc003 Vch { get; set; } = null!;
}
