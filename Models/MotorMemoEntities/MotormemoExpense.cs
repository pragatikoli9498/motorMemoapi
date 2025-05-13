using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class MotormemoExpense
{
    public int DetlId { get; set; }

    public int VchId { get; set; } 

    public int? S_Id { get; set; }

    public int AccCode { get; set; }

    public decimal Charges { get; set; }

    public int Action { get; set; }

    public Boolean IsChecked { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;

    public virtual Motormemo Vch { get; set; } = null!;

    public virtual Sundry? Sundries { get; set; }
    

}
