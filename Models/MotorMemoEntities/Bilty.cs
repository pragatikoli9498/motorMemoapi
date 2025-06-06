namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class Bilty
    {
        public int VchId { get; set; }

        public int? BillNo { get; set; }

        public DateTime? BillDate { get; set; }

        public int FirmId { get; set; }

        public string DivId { get; set; } = null!;

        public DateTime vchDate { get; set; }

        public string From_Dstn { get; set; } = null!;

        public string To_Dstn { get; set; } = null!;

        public int BiltyNo { get; set; }

        public decimal TotalFreight { get; set; }

        public virtual BiltyAudit? BiltyAudit { get; set; }

        public virtual BiltyDetail? BiltyDetails { get; set; }

        public virtual ICollection<BiltyCommodity> BiltyCommodities { get; set; } = new List<BiltyCommodity>();

        public virtual BiltyGstDetails? BiltyGstDetails { get; set; }

        public virtual ICollection<Motormemo2Childe> Motormemo2Childe { get; set; } = new List<Motormemo2Childe>();

    }
}
