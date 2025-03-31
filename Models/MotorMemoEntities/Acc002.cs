using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc002
{
    public int VchId { get; set; }

    public int FirmId { get; set; }
      
    public string? DivId { get; set; }

    public int VchNo { get; set; }

    public string? ChallanNo { get; set; }

    public DateTime VchDate { get; set; }

    public int AccCode { get; set; }

    public decimal Amount { get; set; }

    public string? Nar { get; set; }

    public short TxnType { get; set; }

    public string? TxnNo { get; set; }

    public string? TxnDate { get; set; }

    public string? TxnBywhome { get; set; }

    public string? TxnDrawnon { get; set; }

    public int Against { get; set; }

    public string? RefNo { get; set; }

    public string? RefDate { get; set; }

    public int Rcm { get; set; }

    public int? Postdatedid { get; set; }

    public int? Rtgsid { get; set; }

    public virtual Acc00200? Acc00200 { get; set; }

    public virtual Acc00201? Acc00201 { get; set; }

    //public virtual ICollection<Acc00201> Acc00201s { get; set; } = new List<Acc00201>();

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;
}
