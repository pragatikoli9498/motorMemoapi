using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00601
{
    public int DetlId { get; set; }

    public int VchId { get; set; }

    public int AccCode { get; set; }

    public decimal Amount { get; set; }

    public string? Nar { get; set; }

    public string? AcDate { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;

    public virtual Acc006? Vch { get; set; }
}
