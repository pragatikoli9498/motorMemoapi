using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst00601
{
    public int TalukaId { get; set; }

    public string TalukaName { get; set; } = null!;

    public int DistrictId { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual Mst00602 District { get; set; } = null!;

    public virtual ICollection<Mst006> Mst006s { get; set; } = new List<Mst006>();
}
