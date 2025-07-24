namespace MotorMemo.Models.MotorMemoEntities
{
    public class Mst10806
    {
        public int Detl_Id { get; set; }
        public string VehicleNo { get; set; } = null!;

        public DateTime Eff_Dt { get; set; }

        public string OwnerName { get; set; } = null!;

        public string PanNo { get; set; } = null!;

        public string? BankName { get; set; }

        public string? BankAccNo { get; set; }

        public string? IfscCode { get; set; }

        public int AccCode { get; set; }

        public int IsTransport { get;set; }

        public virtual Mst108? Mst108 { get; set; }

        public virtual Mst011? AccCodeNavigation { get; set; }
    }
}
