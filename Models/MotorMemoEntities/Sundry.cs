using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Sundry
{
    public int S_Id { get; set; }

    public string SundryName { get; set; } = null!;

    public int Operation { get; set; }

    public int ExpaccCode { get; set; }

    public virtual Mst011? AccCodeNavigation { get; set; }


    public virtual ICollection<MotormemoExpense> MotormemoExpenses{ get; set; } = new List<MotormemoExpense>();

    public virtual ICollection<MotormemoOtherCharges> MotormemoOtherCharges { get; set; } = new List<MotormemoOtherCharges>();
}
