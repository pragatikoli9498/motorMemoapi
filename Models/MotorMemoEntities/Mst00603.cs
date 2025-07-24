using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst00603
{
    public int StateCode { get; set; }

    public string StateName { get; set; } = null!;

    public string? StateCapital { get; set; }

    public string? VehicleSeries { get; set; }

    public int? StateUt { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public DateTime? CreatedDt { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public virtual ICollection<Mst00602> Mst00602s { get; set; } = new List<Mst00602>();

    public virtual ICollection<Mst10805> Mst10805s { get; set; } = new List<Mst10805>();

    public virtual ICollection<Tms011> Tms011s { get; set; } = new List<Tms011>();
     
}
