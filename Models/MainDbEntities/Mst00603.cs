using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst00603
{
    public int StateCode { get; set; }

    public string StateName { get; set; } = null!;

    public string? StateCapital { get; set; }

    public string? VehicleSeries { get; set; }

    public bool? StateUt { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; } 

    public virtual ICollection<Mst004> Mst004s { get; set; } = new List<Mst004>();

}
