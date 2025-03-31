using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst01100
{
    public string DivId { get; set; } = null!;

    public int AccCode { get; set; }

    public decimal Crbal { get; set; }

    public decimal Drbal { get; set; }
      
    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public int VchId { get; set; }
  
    public long FirmId { get; set; }
      
    public virtual Mst011 AccCodeNavigation { get; set; } = null!;
}
