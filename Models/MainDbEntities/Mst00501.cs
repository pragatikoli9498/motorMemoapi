using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst00501
{
    public int FirmId { get; set; }

    public string? DivId { get; set; }

    public bool Active { get; set; }

    public string? CfDivId { get; set; }

    public bool TcsApplicable { get; set; }

    public bool Freez { get; set; }

    public bool StkGenType { get; set; }

    public int CalcType { get; set; }

    //public virtual Mst005? Mst005 { get; set;}
}
