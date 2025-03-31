using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst00409
{

    public int FirmId { get; set; }
    public string GstFrom { get; set; } = null!;

    public int GstTyp { get; set; }

    public string GstNo { get; set; } = null!;


     

    public virtual Mst004 Mst004 { get; set; } = null!;
}
