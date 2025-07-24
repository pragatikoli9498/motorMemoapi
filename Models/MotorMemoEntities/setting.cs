namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class setting
    {
        public int Id { get; set; }

        public int SetCode { get; set; }

        public string SetDesc { get; set; } = null!;

        public int? SetValue { get; set; }
    }
}
