using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys00201
{
    public int UserId { get; set; }

    public string? Password { get; set; }

    public virtual Sys00203 User { get; set; } = null!;
}
