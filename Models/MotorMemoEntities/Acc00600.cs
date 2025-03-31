using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00600
{
    public int VchId { get; set; }

    public string? CreatedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedUser { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual Acc006 Vch { get; set; } = null!;
}
