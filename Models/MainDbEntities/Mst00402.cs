using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst00402
{
    public int FirmCode { get; set; }

    public string BranchCode { get; set; } = null!;

    public string BranchName { get; set; } = null!;

    public string BranchAddress1 { get; set; } = null!;

    public string? BranchAddress2 { get; set; }

    public string BranchPlace { get; set; } = null!;

    public int? BranchPincode { get; set; }

    public int BranchStateId { get; set; }

    public string? BranchFno { get; set; }

    public string? BranchEmail { get; set; }

    public string? BranchMobNo { get; set; }

    public string? BranchStartedFrom { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public string? BranchBankAccno { get; set; }

    public string? BranchBankName { get; set; }

    public string? BranchBankIfsc { get; set; }

    public string? SmtpHost { get; set; }

    public string? SmtpPort { get; set; }

    public string? SmtpMailAddress { get; set; }

    public string? SmtpDisplayName { get; set; }

    public string? SmtpUserId { get; set; }

    public string? SmtpPassword { get; set; }

    public string? EinvUrl { get; set; }

    public string? EwayUrl { get; set; }

    public string? EinvKey { get; set; }

    public string? EwayKey { get; set; }

    //public virtual Mst00603 BranchState { get; set; } = null!;

    //public virtual Mst004 FirmCodeNavigation { get; set; } = null!;

    //public virtual ICollection<Mst00403> Mst00403s { get; set; } = new List<Mst00403>();

    //public virtual ICollection<Mst00409> Mst00409s { get; set; } = new List<Mst00409>();

    //public virtual ICollection<Mst005> Mst005s { get; set; } = new List<Mst005>();
}
