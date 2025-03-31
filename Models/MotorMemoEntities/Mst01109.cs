using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst01109
{
    public int AccCode { get; set; }

    public string GstrDate { get; set; } = null!;

    public string Gstur { get; set; } = null!;

    public string? AccGstn { get; set; }
  
    public virtual Mst011? AccCodeNavigation { get; set; }
}
