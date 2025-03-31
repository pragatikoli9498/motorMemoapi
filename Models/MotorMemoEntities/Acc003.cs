using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc003
{
    public int VchId { get; set; }

    public int FirmId { get; set; }
     

    public string DivId { get; set; } = null!;

    public int VchNo { get; set; }

    public string ChallanNo { get; set; } = null!;

    public DateTime VchDate { get; set; }

    public int AccCode { get; set; }

    public decimal Amount { get; set; }

    public string? Nar { get; set; }

    public long TxnType { get; set; }

    public decimal? TxnNo { get; set; }

    public string? TxnDate { get; set; }

    public string? TxnBywhome { get; set; }

    public string? TxnDrawnon { get; set; }

    public int? Against { get; set; }

    public string? RefNo { get; set; }

    public string? RefDate { get; set; }

    public int? Postdatedid { get; set; }

    public int? AppFormNo { get; set; }

    public int? LrId { get; set; }
    
    public int? memoNo { get; set; }

    public virtual Acc00300? Acc00300 { get; set; }

    public virtual Acc00301? Acc00301 { get; set; }

    //public virtual ICollection<Acc00301> Acc00301s { get; set; } = new List<Acc00301>();
         
    public virtual Mst011? AccCodeNavigation { get; set; }

    public virtual Motormemo Motormemo { get; set; } = null!;
}
