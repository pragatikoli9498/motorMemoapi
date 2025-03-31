using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst002
{
    public int GrpCode { get; set; }

    public int MgCode { get; set; }

    public string GrpName { get; set; } = null!;

    public int SrNo { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public DateTime? CreatedDt { get; set; }

    public DateTime? ModifiedDt { get; set; }

    public virtual Mst001? MgCodeNavigation { get; set; } = null!;

    public virtual ICollection<Mst003> Mst003s { get; set; } = new List<Mst003>();
}
