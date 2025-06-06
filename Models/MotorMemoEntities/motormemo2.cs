namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class motormemo2
    {
        public int VchId { get; set; }

        public DateTime VchDate { get; set; }

        public int FirmId { get; set; }

        public string DivId { get; set; } = null!;

        public string From_Dstn { get; set; } = null!;

        public string To_Dstn { get; set; } = null!;

        public string VehicleNo { get; set; } = null!;

        public decimal TotalWet { get; set; }

        public virtual Motormemo2Audit? Motormemo2Audit { get; set; }

        public virtual ICollection<Motormemo2Childe> Motormemo2Childe { get; set; } = new List<Motormemo2Childe>();
    }
}
