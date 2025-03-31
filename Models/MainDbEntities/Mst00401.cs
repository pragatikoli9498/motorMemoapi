using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst00401
{
    public int FirmCode { get; set; }

    public string Logo { get; set; } = null!;

    public virtual Mst004 FirmCodeNavigation { get; set; } = null!;
}
