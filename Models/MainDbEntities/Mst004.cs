using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MainDbEntities;

public partial class Mst004
{
    public int FirmCode { get; set; }

    public string FirmName { get; set; } = null!;

    public string? FirmAlias { get; set; }

    public string FirmAddress1 { get; set; } = null!;
      
    public string FirmPlace { get; set; } = null!;

    public int? FirmPinCode { get; set; }

    public int FirmStateCode { get; set; }

    public string? FirmFno { get; set; }

    public string? EmailId { get; set; }

    public string? WebAddress { get; set; }

    public string? Jurisdiction { get; set; }

    public string? FirmCin { get; set; }

    public string? FirmPan { get; set; }

    public decimal Active { get; set; }

    public string? FirmBankName { get; set; }

    public string? FirmBankAccno { get; set; }

    public string? FirmBankIfsc { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }
      
    public string? FirmMobNo { get; set; }

    public string? FirmLegalName { get; set; }
 
    public virtual Mst00401? Mst00401 { get; set; }

    public virtual ICollection<Mst00403> Mst00403s { get; set; } = new List<Mst00403>();

    public virtual Mst00603? Mst00603 { get; set; }

    public virtual ICollection<Mst005> Mst005s { get; } = new List<Mst005>();
     
    public virtual Mst00409? Mst00409 { get; set; }
     


}
