using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00300
{
    public int VchId { get; set; }

    public string CreatedUser { get; set; } = null!;

    public string CreatedDt { get; set; } = null!;

    public string? ModifiedUser { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual Acc003? Vch { get; set; }
}
