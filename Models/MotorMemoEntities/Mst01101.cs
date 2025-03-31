using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst01101
{
    public int AccCode { get; set; }

    public string? AccAddress { get; set; }

    public string? ContactPerson { get; set; }

    public string? ContactDesignation { get; set; }

    public decimal? ContactMobileNo { get; set; }

    public string? EmailId { get; set; }

    public string? Website { get; set; }

    public decimal? LandlineNo { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;
}
