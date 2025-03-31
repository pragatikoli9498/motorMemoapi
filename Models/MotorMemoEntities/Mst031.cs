using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst031
{
    public long VchId { get; set; }

    public int AccCode { get; set; }

    public string TdsName { get; set; } = null!; 

    public decimal Tds { get; set; }

    public decimal ECess { get; set; }

    public decimal HeCess { get; set; }

    public decimal SfCess { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;
}
