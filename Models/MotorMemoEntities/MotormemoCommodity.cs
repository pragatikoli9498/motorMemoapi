using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class MotormemoCommodity
{
    public int DetlId { get; set; }

    public int VchId { get; set; }

    public string? Commodity { get; set; }

    public string? Uom { get; set; }

    public decimal? Qty { get; set; }

    public decimal? ChrgWeight { get; set; }

    public decimal? ActWeight { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Freight { get; set; }

    public virtual Motormemo Vch { get; set; } = null!;
}
