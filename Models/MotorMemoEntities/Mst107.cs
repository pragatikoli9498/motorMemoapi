using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst107
{
    public int VtypeId { get; set; }

    public string VtypeName { get; set; } = null!;

    public decimal? Capacity { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string CreatedDt { get; set; } = null!;

    public string? ModifiedDt { get; set; }

    public decimal? Vlength { get; set; }

    public decimal? Vheight { get; set; }

    public decimal? Vwidth { get; set; }

    public virtual ICollection<Mst108> Mst108s { get; set; } = new List<Mst108>();
}
