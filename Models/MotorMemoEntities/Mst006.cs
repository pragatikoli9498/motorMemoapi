using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst006
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public int? CityPin { get; set; }

    public int TalukaId { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public DateTime? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public virtual ICollection<Mst011> Mst011s { get; set; } = new List<Mst011>();

    public virtual Mst00601? Taluka { get; set; } = null!;
}
