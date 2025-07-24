using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst108
{
    public string VehicleNo { get; set; } = null!;

    public int VtypeId { get; set; }

    public decimal? CapacityMts { get; set; }
 
    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; } = null!;

    public string? CreatedDt { get; set; } = null!;

    public string? ModifiedDt { get; set; } = null!;

    public int? AccCode { get; set; }

    public string? PanNo { get; set; }

    public string? Alias { get; set; }

    public decimal? CreditLimitAmt { get; set; }

    public string? Enginno { get; set; }

    public string? Chassisno { get; set; }

    public string? DriverAddress { get; set; }

    public string? DriverName { get; set; }

    public decimal? DriverMobileNo { get; set; }

    public string? DriverLicNo { get; set; }

    public string? IsOwn { get; set; }

    public virtual Mst011? AccCodeNavigation { get; set; }

    public virtual Mst10801? Mst10801s { get; set; }
     
    public virtual Mst10803? Mst10803 { get; set; }

    public virtual Mst10804? Mst10804 { get; set; }

    public virtual ICollection<Mst10805> Mst10805s { get; set; } = new List<Mst10805>();

    public virtual Mst107 Vtype { get; set; } = null!;

    public virtual ICollection<Mst10806> Mst10806s { get; set; } = new List<Mst10806>();
}
