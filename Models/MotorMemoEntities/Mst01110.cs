namespace MotorMemo.Models.MotorMemoEntities
{
    public class Mst01110
    {
        public int id { get; set; } 

        public int AccCode { get; set; }

        public int firmCode { get; set; }


        public virtual Mst011 AccCodeNavigation { get; set; } = null!;
    }
}
