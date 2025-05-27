using System.Security.Principal;

namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class BiltyGstDetails
    {
        public int DetlId { get; set; }

        public int VchId { get; set; }

        public DateTime EffDate { get; set; }

        public decimal Igst { get; set; }

        public decimal Cgst { get; set; }

        public decimal Sgst { get; set; }

        public decimal Cess { get; set; }

        public decimal GstAmt { get; set; }

        public decimal TotalAmt { get; set; }

        public decimal IgstAmt { get; set; }

        public decimal CgstAmt { get; set; }

        public decimal SgstAmt { get; set; }

        public decimal CessAmt { get; set; }

        public virtual Bilty Bilty { get; set; }= null!;
    }
}
