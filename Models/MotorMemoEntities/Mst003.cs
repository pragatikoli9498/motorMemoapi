using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst003
{
    public int SgCode { get; set; }

    public int GrpCode { get; set; }

    public string SgName { get; set; } = null!;

    public int? SrNo { get; set; }

    public int Show { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public DateTime? CreatedDt { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public virtual Mst002? GrpCodeNavigation { get; set; } = null!;

    public virtual ICollection<Mst011> Mst011s { get; set; } = new List<Mst011>();
}
