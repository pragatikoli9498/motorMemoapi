using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys00204
{
    public int UserId { get; set; }

    public decimal? A { get; set; }

    public decimal? E { get; set; }

    public decimal? D { get; set; }

    public decimal? L { get; set; }

    public decimal? P { get; set; }

    public decimal? Sysadmin { get; set; }

    public decimal? O { get; set; }

    public decimal? J { get; set; }

    public decimal? EAdmin { get; set; }

    public virtual Sys00203 User { get; set; } = null!;
}
