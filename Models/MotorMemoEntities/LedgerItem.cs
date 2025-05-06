namespace MotorMemo.Models.MotorMemoEntities
{
    public class LedgerItem
    {
        public short vch_type { get; set; }
        public string vch_type_name { get; set; }
        public string challan_no { get; set; }
        public DateTime vch_date { get; set; }
        public string acc_name { get; set; }
        public decimal cramt { get; set; }
        public decimal dramt { get; set; }
        public string refName { get; set; }
    }
}
