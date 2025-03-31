using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst01115
{
    public int DivId { get; set; }

    public int AccCode { get; set; }

    public string TcsApplicable { get; set; } = null!;

    public string TdsApplicable { get; set; } = null!;

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;
}
