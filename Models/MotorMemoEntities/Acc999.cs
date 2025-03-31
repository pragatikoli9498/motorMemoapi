using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc999
{
    public int VchId { get; set; }

    public long FirmId { get; set; }

    public string? BranchId { get; set; }

    public string DivId { get; set; } = null!;

    public short VchType { get; set; }

    public int ChallanId { get; set; }

    public int? VchNo { get; set; }

    public DateTime VchDate { get; set; }

    public string? ChallanNo { get; set; }

    public string? Nar { get; set; }

    public string? Linkedchallan { get; set; }

    public int? SupplierCode { get; set; }

    public string? Inventory { get; set; }

    public int? AccCode { get; set; }

    public DateTime? DueDate { get; set; }

    public bool Approval { get; set; }

    public virtual ICollection<Acc99901> Acc99901s { get; set; } = new List<Acc99901>();
}
