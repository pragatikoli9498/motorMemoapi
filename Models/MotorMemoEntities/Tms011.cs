namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class Tms011
    {
        public int VchId { get; set; }

        public DateTime VchDt { get; set; }

        public int VchNo { get; set; }

        public int FirmId { get; set; }

        public string DivId { get; set; } = null!;

        public string BillNo { get; set; } = null!;

        public DateTime? BillDt { get; set; }

        public int IsRcm { get; set; }

        public DateTime FromDt { get; set; }

        public DateTime ToDt { get; set; }  

        public int AccCode { get; set; }

        public decimal? CgstRate { get; set; }

        public decimal? CgstAmt { get; set; }

        public decimal? SgstRate { get; set; }

        public decimal? SgstAmt { get; set; }

        public decimal? IgstRate { get; set; }

        public decimal? IgstAmt { get; set; }

        public string Sac { get; set; } = null!;

        public decimal? RoundOff { get; set; }

        public int StateCode { get; set; }

        public decimal? GrossAmt { get; set; }

        public decimal? NetAmt { get; set; }

        public virtual Mst011? AccCodeNavigation { get; set; }

        public virtual ICollection<Tms011_01> Tms01101s { get; set; } = new List<Tms011_01>();

        public virtual Mst00603? Mst00603 { get; set; }

     }
}
