using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst012
{
    public string UnitCode { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual ICollection<Mst010> Mst010s { get; set; } = new List<Mst010>();
}
