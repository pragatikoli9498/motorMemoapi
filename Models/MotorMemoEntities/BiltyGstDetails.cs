using System.Security.Principal;

namespace MotorMemo.Models.MotorMemoEntities
{
    public partial class BiltyGstDetails
    {
        public int DetlId { get; set; }

        public int VchId { get; set; }

        public DateTime EffDate { get; set; }

        public decimal IGST { get; set; }

        public decimal CGST { get; set; }

        public decimal SGST { get; set; }

        public decimal CESS { get; set; }

        public virtual Bilty Bilty { get; set; } = new Bilty();
    }
}
