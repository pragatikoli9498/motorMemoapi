using System;
using System.Collections.Generic;
namespace MotorMemo.Models.MotorMemoEntities
{
    public class Mst10301
    {
        public int detlid { get; set; }
        public int Id { get; set; }

        public string VehicleNo { get; set; }
        

        public virtual Mst103? Vch { get; set; }
    }
}
