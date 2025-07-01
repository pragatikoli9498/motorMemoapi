using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00301
{
    public int DetlId { get; set; }

    public int VchId { get; set; }

    public int AccCode { get; set; }

    public string Amount { get; set; } = null!;

    public string TdsRate { get; set; } = null!;

    public decimal TdsAmt { get; set; }
    public decimal RecAmt { get; set; }

    public int? CostId { get; set; }

    public DateTime AcDate { get; set; }
     
    public virtual Mst011? AccCodeNavigation { get; set; } 

    public virtual Acc003? Vch { get; set; }
}
