using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00302
{
    public int DetlId { get; set; }

    public int VchId { get; set; }

    public string? Hsncode { get; set; }

    public string Description { get; set; } = null!;

    public decimal Taxableamt { get; set; }

    public decimal Srate { get; set; }

    public decimal Crate { get; set; }

    public decimal Irate { get; set; }

    public decimal Csrate { get; set; }

    public decimal Samt { get; set; }

    public decimal Camt { get; set; }

    public decimal Iamt { get; set; }

    public decimal Csamt { get; set; }

    public decimal Totalamt { get; set; }

    public virtual Acc003 Vch { get; set; } = null!;
}
