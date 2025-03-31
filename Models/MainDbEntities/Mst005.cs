using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst005
{
    public int FirmCode { get; set; }

    //public string BranchCode { get; set; } = null!;

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

    //public virtual Mst00402 Mst00402 { get; set; } = null!;

    public virtual Mst004? Mst004 { get; set; }

    //public virtual ICollection<Mst00501> Mst00501 { get; set; } =new List<Mst00501>(); 



}
