using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst005
{
    public int FirmCode { get; set; }

    public string DivId { get; set; } = null!;

    public string? FromDivId { get; set; }

    public DateTime Fdt { get; set; } 

    public DateTime Tdt { get; set; } 

    public string Prefix { get; set; } = null!;

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public decimal? IsprevYr { get; set; }

    public virtual Mst004? Mst004 { get; set; }

   
}
