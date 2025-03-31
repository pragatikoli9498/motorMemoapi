using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc99901
{
    public int VchId { get; set; }

    public int AccCode { get; set; }

    public int? RefCode { get; set; }

    public decimal Dramt { get; set; }

    public decimal Cramt { get; set; }

    public string? AcDate { get; set; }

    public string? Nar { get; set; }

    public int DetlId { get; set; }

    public int? ProdCode { get; set; }

    public string? Against { get; set; }

    public virtual Mst011? AccCodeNavigation { get; set; } = null!;

    public virtual Acc999 Vch { get; set; } = null!;
}
