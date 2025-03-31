using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00307
{
    public int VchId { get; set; }

    public string? ApprovedBy { get; set; }

    public string? ApprovedDt { get; set; }

    public virtual Acc003 Vch { get; set; } = null!;
}
