using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Sys010
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int? AccCode { get; set; }

    public virtual Mst011? AccCodeNavigation { get; set; }
}
