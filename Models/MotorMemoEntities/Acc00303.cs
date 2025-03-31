using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00303
{
    public int VchId { get; set; }

    public int Linkid { get; set; }

    public decimal LinkedAmt { get; set; }

    public int DetlId { get; set; }

    public virtual Acc003 Vch { get; set; } = null!;
}
