using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst030
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Placeid { get; set; }

    public string? pincode { get; set; }

    public string? GstinNo { get; set; }

    public int? StateCode { get; set; }

    public string? MobileNo { get; set; }

    public string? EmailId { get; set; }

    public int? AccCode { get; set; }

    public string? Address { get; set; }
      
    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual Mst011? AccCodeNavigation { get; set; }

    public virtual Mst006? PlaceIdNavigation { get; set; }
}
