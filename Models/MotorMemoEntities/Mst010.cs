using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst010
{
    public int IId { get; set; }

    public string IName { get; set; } = null!;

    public string? IUnit { get; set; }

    public string? IHsnDescription { get; set; }

    public string? IHsn { get; set; }

    public decimal? IGst { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual Mst012? IUnitNavigation { get; set; }
}
