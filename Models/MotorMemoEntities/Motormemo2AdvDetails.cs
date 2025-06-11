namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class Motormemo2AdvDetails
    {
        public int DetlId { get; set; }

        public int VchId { get; set; }

        public int AccCode { get; set; }

        public decimal Amount { get; set; }

        public string? Narration { get; set; }

        public virtual motormemo2? Motormemo2 { get; set; }

        public virtual Mst011? AccCodeNavigation { get; set; }
    }
}
