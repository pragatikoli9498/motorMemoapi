namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class Motormemo2Childe
    {
        public int DetlId { get; set; }

        public int VchId { get; set; }

        public int BiltyId { get; set; }

        public decimal Weight { get; set; }

        public int? EwayNo { get; set; } 

        public string? ValidUpTo { get; set; }



        public virtual motormemo2? Motormemo2 { get; set; }

        public virtual Bilty? Bilty { get; set; } 
    }
}
