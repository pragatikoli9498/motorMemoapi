using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Sys001
{
    public int Id { get; set; }

    public int DescKey { get; set; }

    public string DescName { get; set; } = null!;

    public string? DescValue { get; set; }
}
