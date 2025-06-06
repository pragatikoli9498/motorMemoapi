namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class Motormemo2Audit
    {
        public int VchId { get; set; }

        public string? CreatedUser { get; set; }

        public string? CreatedDt { get; set; }

        public string? ModifiedUser { get; set; }

        public string? ModifiedDt { get; set; }

        public virtual motormemo2 Motormemo2 { get; set; } = null!;
    }
}
