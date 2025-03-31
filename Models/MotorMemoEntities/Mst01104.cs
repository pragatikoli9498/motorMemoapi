using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst01104
{
    public int AccCode { get; set; }

    public string? BankName { get; set; }

    public string? Address { get; set; }

    public string? BankaccNo { get; set; }

    public string? IfscCode { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;
}
