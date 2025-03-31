using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys003
{
    public int EndId { get; set; }

    public string EndDate { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public int? FirmCode { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? ModifiedDt { get; set; }

    public byte[]? CreatedDt { get; set; }

    public int Status { get; set; }
}
