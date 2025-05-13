namespace MotorMemo.Models.MotorMemoEntities
{
    public class MotormemoOtherCharges
    {
        public int DetlId { get; set; }

        public int VchId { get; set; }
         
        public string? otherchag { get; set; }

        public int? S_Id { get; set; }

        public int AccCode { get; set; }

        public virtual Mst011? AccCodeNavigation { get; set; } 

        public virtual Sundry? Sundries { get; set; } 

        public virtual Motormemo Vch { get; set; } = null!;
    }
}
