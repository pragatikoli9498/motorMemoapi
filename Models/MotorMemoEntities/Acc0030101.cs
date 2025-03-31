using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc0030101
{
    public int DetlId { get; set; }

    public int VchType { get; set; }

    public decimal ChallanNo { get; set; }

    public decimal? BillNo { get; set; }

    public int RefDetlId { get; set; }

    public string? BillDate { get; set; }

    public decimal? Amount { get; set; }
     
}
