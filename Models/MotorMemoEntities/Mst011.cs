using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Mst011
{
    public int AccCode { get; set; }

    public int SgCode { get; set; }

    public string AccName { get; set; } = null!;

    public string? AccAlias { get; set; }

    public string? CreatedUser { get; set; }

    public string? ModifiedUser { get; set; }

    public string? CreatedDt { get; set; }

    public string? ModifiedDt { get; set; }

    public int? TanNo { get; set; }

    public string? PanNo { get; set; }

    public string? CreditLimit { get; set; }

    public string? CinNo { get; set; }

    public int? PlaceId { get; set; }

    public Boolean? IsDisabled { get; set; }

    public virtual ICollection<Acc00201> Acc00201s { get; set; } = new List<Acc00201>();

    public virtual ICollection<Acc002> Acc002s { get; set; } = new List<Acc002>();

    public virtual ICollection<Acc00301> Acc00301s { get; set; } = new List<Acc00301>();

    public virtual ICollection<Acc003> Acc003s { get; set; } = new List<Acc003>();

    public virtual ICollection<Acc00501> Acc00501s { get; set; } = new List<Acc00501>();

    public virtual ICollection<Acc005> Acc005s { get; set; } = new List<Acc005>();

    public virtual ICollection<Acc00601> Acc00601s { get; set; } = new List<Acc00601>();

    public virtual ICollection<Acc006> Acc006s { get; set; } = new List<Acc006>();

    public virtual ICollection<Acc99901> Acc99901s { get; set; } = new List<Acc99901>();

    public virtual ICollection<MotormemoExpense> MotormemoExpenses { get; set; } = new List<MotormemoExpense>();

    public virtual ICollection<MotormemoOtherCharges> MotormemoOtherCharges { get; set; } = new List<MotormemoOtherCharges>();

    public virtual ICollection<Mst01100> Mst01100s { get; set; } = new List<Mst01100>();

    public virtual ICollection<Mst01110> Mst01110s { get; set; } = new List<Mst01110>();

    public virtual Mst01101? Mst01101 { get; set; }
      
    public virtual Mst01104? Mst01104 { get; set; }

    public virtual Mst01109? Mst01109 { get; set; }

    public virtual ICollection<Mst01115> Mst01115s { get; set; } = new List<Mst01115>();

    public virtual ICollection<Mst031> Mst031s { get; set; } = new List<Mst031>();

    public virtual ICollection<Mst103> Mst103s { get; set; } = new List<Mst103>();

    public virtual ICollection<Mst10803>  ExpAccCodeNavigations { get; set; } = new List<Mst10803>();

   

    public virtual ICollection<Mst108> Mst108s { get; set; } = new List<Mst108>();

    public virtual Mst006? Place { get; set; }

    public virtual Mst003? SgCodeNavigation { get; set; } = null!;

    public virtual ICollection<Sundry> Sundries { get; set; } = new List<Sundry>();
     

    public virtual ICollection<Sys010> Sys010s { get; set; } = new List<Sys010>();
}
