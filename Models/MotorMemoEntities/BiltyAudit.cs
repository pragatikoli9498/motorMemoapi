namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class BiltyAudit
    {
        public int VchId { get; set; }

        public string? CreatedUser { get; set; }

        public string? CreatedDt { get; set; }

        public string? ModifiedUser { get; set; }

        public string? ModifiedDt { get; set; }

        public virtual Bilty Bilty { get; set; } = null!;
    }
}
