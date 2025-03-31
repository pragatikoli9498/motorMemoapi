using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst001
{
    public int MgCode { get; set; }

    public string MgName { get; set; } = null!;

    public int MgType { get; set; }

    public int? MgAlias { get; set; }

    public string? MgHead { get; set; }

    public int? MgBs { get; set; }

    public virtual ICollection<Mst002> Mst002s { get; set; } = new List<Mst002>();
}
