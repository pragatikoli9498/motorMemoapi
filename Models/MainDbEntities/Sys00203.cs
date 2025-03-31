using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Sys00203
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserLongname { get; set; }

    public int RoleId { get; set; }

    public string? Mobileno { get; set; }

    public string? Token { get; set; }

    public string? Otp { get; set; }

    public string? OtpExpiry { get; set; }

    public string? EmailId { get; set; }

    public int? LocalIp { get; set; }

    public string? PublicIp { get; set; }

    public virtual Sys00202 Role { get; set; } = null!;

    public virtual Sys00201? Sys00201 { get; set; }

    public virtual Sys00204? Sys00204 { get; set; }

    public virtual ICollection<Sys00207> Sys00207s { get; set; } = new List<Sys00207>();
}
