using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc00309
{
    public int DetlId { get; set; }

    public int VchId { get; set; }

    public string? Attachdescr { get; set; }

    public string? Filepath { get; set; }

    public string? Uploadby { get; set; }

    public string? UploadedDt { get; set; }

    public virtual Acc003 Vch { get; set; } = null!;
}
