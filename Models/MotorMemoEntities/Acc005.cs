using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc005
{
    public int VchId { get; set; }

    public int FirmId { get; set; }
      
    public string? DivId { get; set; }

    public int VchNo { get; set; }

    public string? ChallanNo { get; set; }

    public DateTime VchDate { get; set; }

    public int AccCode { get; set; }

    public decimal Amount { get; set; }

    public string? AcDate { get; set; }

    public string? Nar { get; set; }

    public string TransType { get; set; } = null!;

    public int Against { get; set; }

    public decimal InvType { get; set; }

    public virtual Acc00500? Acc00500 { get; set; }

    public virtual ICollection<Acc00501> Acc00501s { get; set; } = new List<Acc00501>();

    public virtual Acc00507? Acc00507 { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;
}
