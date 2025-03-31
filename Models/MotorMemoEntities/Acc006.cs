using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc006
{
    public int VchId { get; set; }

    public int FirmId { get; set; }
     

    public string DivId { get; set; } = null!;

    public int VchNo { get; set; }

    public string? ChallanNo { get; set; } = null!;

    public DateTime VchDate { get; set; }
    public int AccCode { get; set; }

    public decimal Amount { get; set; }

    public DateTime AcDate { get; set; }

    public string? Nar { get; set; }

    public long TransType { get; set; }  

    public string? Cashier { get; set; }

    public virtual Acc00600? Acc00600 { get; set; }

    public virtual ICollection<Acc00601> Acc00601s { get; set; } = new List<Acc00601>();

    public virtual Acc00607? Acc00607 { get; set; }

    public virtual Mst011? AccCodeNavigation { get; set; } = null!;
}
