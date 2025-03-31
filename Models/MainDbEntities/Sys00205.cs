using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys00205
{
    public int UserId { get; set; }

    public decimal? A { get; set; }

    public decimal? E { get; set; }

    public decimal? D { get; set; }

    public decimal? L { get; set; }

    public decimal? P { get; set; }

    public decimal? B { get; set; }

    public decimal? R { get; set; }

    public decimal? O { get; set; }

    public virtual Sys00202 User { get; set; } = null!;
}
