using System;
using System.Collections.Generic;

namespace MotorMemo.Models.MotorMemoEntities;

public partial class Acc001
{
    public int? VchId { get; set; }

    public int? FirmId { get; set; }

    public int? BranchId { get; set; }

    public int? DivId { get; set; }

    public string? VchNo { get; set; }

    public string? ChallanNo { get; set; }

    public DateTime? VchDate { get; set; }

    public int? AccCode { get; set; }

    public int? IsRcm { get; set; }

    public string? Nar { get; set; }

    public string? MemoVchId { get; set; }

    public int? TaxableAmt { get; set; }

    public double? TotalSamt { get; set; }

    public double? TotalCamt { get; set; }

    public int? TotalIamt { get; set; }

    public string? TotalCsAmt { get; set; }

    public double? RndAmt { get; set; }

    public int? NetAmt { get; set; }

    public int? ReasonIssueNote { get; set; }

    public string? CrNoteNo { get; set; }

    public DateTime? CrNoteDate { get; set; }

    public string? LinkId { get; set; }

    public int? DebitAmt { get; set; }

    public string? TotalStateCsAmt { get; set; }

    public string? TotalNonAdvlAmt { get; set; }

    public string? TotalStateNonAdvlAmt { get; set; }

    public string? TotalItemOthCharges { get; set; }

    public string? OthCharges { get; set; }

    public string? GrossAmt { get; set; }

    public int? BillNo { get; set; }

    public DateOnly? BillDate { get; set; }

    public int? Against { get; set; }

    public int? TotalGst { get; set; }

    public int? TotalCs { get; set; }

    public int? TotalTaxablevalue { get; set; }

    public string? BarCode { get; set; }

    public string? DiscAmtTotal { get; set; }

    public string? DiffAmtTotal { get; set; }
}
