﻿using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class MotormemoDetail
{
    public int DetlId { get; set; }

    public int VchId { get; set; }

    public string? SenderGstin { get; set; }

    public string? SenderMobileNo { get; set; }

    public string SenderName { get; set; } = null!;

    public string SenderAddress1 { get; set; } = null!;

    public string SenderPin { get; set; } = null!;

    public int SenderStateId { get; set; }

    public string? SenderMail { get; set; }

    public string SenderBillNo { get; set; } = null!;

    public string SenderBillDt { get; set; } = null!;

    public string? ReceiverGstin { get; set; }

    public string? ReceiverMobileNo { get; set; }

    public string ReceiverName { get; set; } = null!;

    public string ReceiverAddress { get; set; } = null!;

    public string ReceiverPin { get; set; } = null!;

    public int ReceiverStateId { get; set; }

    public string ReceiverMail { get; set; } = null!;

    public string? EwayNo { get; set; }

    public int? senderAccount { get; set; }

    public int? senderAmount { get; set; }

    public int? receiverAccount { get; set; }

    public int? receiverAmount { get; set; }
     
    public int? ownerAccount { get; set; }

    public virtual Motormemo Vch { get; set; } = null!;
     
}
