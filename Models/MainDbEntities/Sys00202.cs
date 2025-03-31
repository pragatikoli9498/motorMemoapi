using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys00202
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Comments { get; set; }

    public virtual ICollection<Sys00203> Sys00203s { get; set; } = new List<Sys00203>();

    public virtual Sys00205? Sys00205 { get; set; }
}
