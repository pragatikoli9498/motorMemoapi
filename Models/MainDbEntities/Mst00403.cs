using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst00403
{
    public int LicId { get; set; }

    public int FirmCode { get; set; } 

    public string LicName { get; set; } = null!;

    public string LicNo { get; set; } = null!;

    public virtual Mst004 Mst004 { get; set; } = null!;
}
