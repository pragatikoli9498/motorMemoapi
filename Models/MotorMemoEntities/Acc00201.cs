using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00201
{
    public int DetlId { get; set; }

    public int? VchId { get; set; }

    public int AccCode { get; set; }

    public decimal? Amount { get; set; }

    public decimal TdsRate { get; set; }

    public decimal TdsAmt { get; set; }

    public decimal RecAmt { get; set; }

    public int? CostId { get; set; }

    public string? AcDate { get; set; }

    public string? Nar { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;

    public virtual Acc002? Vch { get; set; }
}
