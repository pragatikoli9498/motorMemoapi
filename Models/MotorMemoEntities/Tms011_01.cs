namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class Tms011_01
    {
        public int DetlId { get; set; }

        public int VchId { get; set; }

        public int LrId { get; set; }

        public DateTime? Dt { get; set; }

        public string? From_Dstn { get; set; }

        public string? To_Dstn { get; set; }

        public string VehicleNo { get; set; } = null!;

        public int? MemoNo { get; set; }

        public int? BillAmt { get; set; }

        public int? KiloMiter { get; set; }

        public virtual Tms011? Tms011 { get; set; }

        public virtual Motormemo? Motormemo { get; set; }
    }
}
