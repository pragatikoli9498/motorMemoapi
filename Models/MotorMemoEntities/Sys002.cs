using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Sys002
{
    public int VchType { get; set; }

    public string VchTypeName { get; set; } = null!;

    public int SrNo { get; set; }

    public string? Description { get; set; }

    public string? MsgString { get; set; }

    public string? Variables { get; set; }

    public string? Prefix { get; set; }

    public string? Pattern { get; set; }

    public bool Padding { get; set; }  

    public int VchLength { get; set; }

    public bool AutoNo { get; set; }  

    public string InclInDayend { get; set; } = null!;
}
