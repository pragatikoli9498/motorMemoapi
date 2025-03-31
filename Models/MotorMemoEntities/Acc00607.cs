using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00607
{
    public int VchId { get; set; }

    public string? ApprovedBy { get; set; }

    public string? ApprovedDt { get; set; }

    public virtual Acc006 Vch { get; set; } = null!;
}
