using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst10300
{
    public int VchId { get; set; }

    public string? CreatedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedUser { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual Mst103 Vch { get; set; } = null!;
}
