using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Sys200
{
    public int Id { get; set; }

    public string EntityName { get; set; } = null!;

    public int FirmId { get; set; }

    public string BranchId { get; set; } = null!;

    public string PmKey { get; set; } = null!;

    public string ChallanNo { get; set; } = null!;

    public string PropertyName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string DateChanged { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Status { get; set; }
}
