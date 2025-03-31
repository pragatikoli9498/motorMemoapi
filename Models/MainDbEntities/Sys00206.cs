using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys00206
{
    public int Id { get; set; }

    public string Modulename { get; set; } = null!;

    public virtual ICollection<Sys00207> Sys00207s { get; set; } = new List<Sys00207>();
}
