using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys00207
{
    public int? UserId { get; set; }

    public int? ModuleId { get; set; }

    public int Id { get; set; }

    public virtual Sys00206? Module { get; set; }

    public virtual Sys00203? User { get; set; }
}
