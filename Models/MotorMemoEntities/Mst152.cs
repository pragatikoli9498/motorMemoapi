using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst152
{
    public int FtId { get; set; }

    public string FtName { get; set; } = null!;

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public string? DealerCtg { get; set; }
     
}
