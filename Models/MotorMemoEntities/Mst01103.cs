using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst01103
{
    public int AccCode { get; set; }

    public decimal? CreditLimit { get; set; }

    public string? CreditLimitDays { get; set; }

    public int? FirmType { get; set; }

    public string? AccPanNo { get; set; }

    public string? AccTanNo { get; set; }

    public string? AccCinNo { get; set; }

    public string? CurrencyCode { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;

    public virtual Mst152? FirmTypeNavigation { get; set; }
}
