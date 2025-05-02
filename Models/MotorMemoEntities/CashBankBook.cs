namespace MotorMemo.Models.MotorMemoEntities
{
    public class CashBankBook
    {
        public long? acc_code { get; set; }

        public string acc_name { get; set; }

        public short? vch_type { get; set; }

        public string vch_type_name { get; set; }

        public string challan_no { get; set; }

        public DateTime? vch_date { get; set; }

        public int? ref_code { get; set; }

        public long? vch_id { get; set; }

        public string refName { get; set; }

        public string Naration { get; set; }

        public int? vch_no { get; set; }

        public double? balance { get; set; }

        public double? dramt { get; set; }

        public double? cramt { get; set; }

        public long? challan_id { get; set; }

        public string email_id { get; set; }

        public string contact_mobile_no { get; set; }
    }
}
