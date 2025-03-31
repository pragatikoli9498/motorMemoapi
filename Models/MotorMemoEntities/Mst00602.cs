using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst00602
{
    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public int StateCode { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual ICollection<Mst00601> Mst00601s { get; set; } = new List<Mst00601>();

    public virtual Mst00603 StateCodeNavigation { get; set; } = null!;
}
