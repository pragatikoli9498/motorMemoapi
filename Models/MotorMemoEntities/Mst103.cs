using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst103
{
    public int DeclrId { get; set; }

    public string? FyId { get; set; } 

    public string? DeclrNo { get; set; } = null!;

    public int AccCode { get; set; }

    public string FromDt { get; set; } = null!;

    public string? PanNo { get; set; }

    public int NoOfVehicles { get; set; }

    public string Ishuf { get; set; }

    public virtual Mst011 AccCodeNavigation { get; set; } = null!;

    public virtual Mst10300? Mst10300 { get; set; }

    public virtual ICollection<Mst10301> Mst10301s { get; set; } = new List<Mst10301>();
}
