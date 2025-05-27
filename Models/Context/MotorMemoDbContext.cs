using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.MotorMemoEntities;

namespace MotorMemo.Models.Context;

public partial class MotorMemoDbContext : DbContext
{
    public MotorMemoDbContext()
    {
    }

    public MotorMemoDbContext(DbContextOptions<MotorMemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acc001> Acc001s { get; set; }

    public virtual DbSet<Acc002> Acc002s { get; set; }

    public virtual DbSet<Acc00200> Acc00200s { get; set; }

    public virtual DbSet<Acc00201> Acc00201s { get; set; }

    public virtual DbSet<Acc003> Acc003s { get; set; }

    public virtual DbSet<Acc00300> Acc00300s { get; set; }

    public virtual DbSet<Acc00301> Acc00301s { get; set; }
          
    public virtual DbSet<Acc005> Acc005s { get; set; }

    public virtual DbSet<Acc00500> Acc00500s { get; set; }

    public virtual DbSet<Acc00501> Acc00501s { get; set; }

    public virtual DbSet<Acc00507> Acc00507s { get; set; }

    public virtual DbSet<Acc006> Acc006s { get; set; }

    public virtual DbSet<Acc00600> Acc00600s { get; set; }

    public virtual DbSet<Acc00601> Acc00601s { get; set; }

    public virtual DbSet<Acc00607> Acc00607s { get; set; }

    public virtual DbSet<Acc999> Acc999s { get; set; }

    public virtual DbSet<Acc99901> Acc99901s { get; set; }

    public virtual DbSet<Bilty> Bilties { get; set; }

    public virtual DbSet<BiltyAudit> BiltyAudits { get; set; }

    public virtual DbSet<BiltyCommodity> BiltyCommodities { get; set; }

    public virtual DbSet<BiltyDetail> BiltyDetails { get; set; }

    public virtual DbSet<BiltyGstDetails> BiltyGstDetails { get; set; }

    public virtual DbSet<Motormemo> Motormemos { get; set; }

    public virtual DbSet<MotormemoAudit> MotormemoAudits { get; set; }

    public virtual DbSet<MotormemoCommodity> MotormemoCommodities { get; set; }

    public virtual DbSet<MotormemoDetail> MotormemoDetails { get; set; }

    public virtual DbSet<MotormemoExpense> MotormemoExpenses { get; set; }

    public virtual DbSet<MotormemoOtherCharges> MotormemoOtherCharges { get; set; }

    public virtual DbSet<Mst001> Mst001s { get; set; }

    public virtual DbSet<Mst002> Mst002s { get; set; }

    public virtual DbSet<Mst003> Mst003s { get; set; }

    public virtual DbSet<Mst006> Mst006s { get; set; }

    public virtual DbSet<Mst00601> Mst00601s { get; set; }

    public virtual DbSet<Mst00602> Mst00602s { get; set; }

    public virtual DbSet<Mst00603> Mst00603s { get; set; }

    public virtual DbSet<Mst010> Mst010s { get; set; }

    public virtual DbSet<Mst011> Mst011s { get; set; }

    public virtual DbSet<Mst01100> Mst01100s { get; set; }

    public virtual DbSet<Mst01101> Mst01101s { get; set; }
     

    public virtual DbSet<Mst01104> Mst01104s { get; set; }

    public virtual DbSet<Mst01109> Mst01109s { get; set; }

    public virtual DbSet<Mst01110> Mst01110s { get; set; }

    public virtual DbSet<Mst01115> Mst01115s { get; set; }

    public virtual DbSet<Mst012> Mst012s { get; set; }

    public virtual DbSet<Mst030> Mst030s { get; set; }

    public virtual DbSet<Mst031> Mst031s { get; set; }
     
    public virtual DbSet<Mst103> Mst103s { get; set; }

    public virtual DbSet<Mst10300> Mst10300s { get; set; }

    public virtual DbSet<Mst10301> Mst10301s { get; set; }

    public virtual DbSet<Mst107> Mst107s { get; set; }

    public virtual DbSet<Mst108> Mst108s { get; set; }
 

    public virtual DbSet<Mst10801> Mst10801s { get; set; }
      
    public virtual DbSet<Mst10803> Mst10803s { get; set; }

    public virtual DbSet<Mst10804> Mst10804s { get; set; }

    public virtual DbSet<Mst10805> Mst10805s { get; set; }

    public virtual DbSet<Mst152> Mst152s { get; set; }

    public virtual DbSet<Sundry> Sundries { get; set; }
      
    public virtual DbSet<Sys001> Sys001s { get; set; }

    public virtual DbSet<Sys002> Sys002s { get; set; }

    public virtual DbSet<Sys010> Sys010s { get; set; }

    public virtual DbSet<Sys200> Sys200s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acc001>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("acc001");

            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Against).HasColumnName("against");
            entity.Property(e => e.BarCode).HasColumnName("barCode");
            entity.Property(e => e.BillDate).HasColumnName("billDate");
            entity.Property(e => e.BillNo).HasColumnName("billNo");
            entity.Property(e => e.BranchId)
                .HasColumnType("NUMERIC")
                .HasColumnName("branch_id");
            entity.Property(e => e.ChallanNo).HasColumnName("challan_no");
            entity.Property(e => e.CrNoteDate).HasColumnName("cr_note_date");
            entity.Property(e => e.CrNoteNo).HasColumnName("cr_note_no");
            entity.Property(e => e.DebitAmt).HasColumnName("debitAmt");
            entity.Property(e => e.DiffAmtTotal).HasColumnName("diffAmtTotal");
            entity.Property(e => e.DiscAmtTotal).HasColumnName("discAmtTotal");
            entity.Property(e => e.DivId).HasColumnName("div_id");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.GrossAmt).HasColumnName("grossAmt");
            entity.Property(e => e.IsRcm).HasColumnName("isRcm");
            entity.Property(e => e.LinkId).HasColumnName("linkId");
            entity.Property(e => e.MemoVchId).HasColumnName("memo_vch_id");
            entity.Property(e => e.Nar).HasColumnName("nar");
            entity.Property(e => e.OthCharges).HasColumnName("oth_charges");
            entity.Property(e => e.ReasonIssueNote).HasColumnName("reason_issue_note");
            entity.Property(e => e.RndAmt)
                .HasColumnType("INTEGER")
                .HasColumnName("rndAmt");
            entity.Property(e => e.TotalCamt).HasColumnType("INTEGER");
            entity.Property(e => e.TotalCs).HasColumnName("totalCs");
            entity.Property(e => e.TotalGst).HasColumnName("totalGst");
            entity.Property(e => e.TotalSamt).HasColumnType("INTEGER");
            entity.Property(e => e.TotalTaxablevalue).HasColumnName("totalTaxablevalue");
            entity.Property(e => e.VchDate).HasColumnName("vch_date");
            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.VchNo).HasColumnName("vch_no");
        });

        modelBuilder.Entity<Acc002>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc002");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Against).HasColumnName("against");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMERIC")
                .HasColumnName("amount");
      
            entity.Property(e => e.ChallanNo)
                .UseCollation("NOCASE")
                .HasColumnType("NUMERIC")
                .HasColumnName("challan_no");
            entity.Property(e => e.DivId)
                .UseCollation("NOCASE")
                .HasColumnName("div_id");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.Nar).UseCollation("NOCASE");
            entity.Property(e => e.Postdatedid).HasColumnName("postdatedid");
            entity.Property(e => e.Rcm).HasColumnName("rcm");
            entity.Property(e => e.RefDate)
                .UseCollation("NOCASE")
                .HasColumnName("ref_date");
            entity.Property(e => e.RefNo)
                .UseCollation("NOCASE")
                .HasColumnName("ref_no");
            entity.Property(e => e.Rtgsid).HasColumnName("rtgsid");
            entity.Property(e => e.TxnBywhome)
                .UseCollation("NOCASE")
                .HasColumnName("txn_bywhome");
            entity.Property(e => e.TxnDate).HasColumnName("txn_date");
            entity.Property(e => e.TxnDrawnon)
                .UseCollation("NOCASE")
                .HasColumnName("txn_drawnon");
            entity.Property(e => e.TxnNo)
                .UseCollation("NOCASE")
                .HasColumnType("NVARCHAR(50)")
                .HasColumnName("txn_no");
            entity.Property(e => e.TxnType)
                .HasColumnType("SMALLINT")
                .HasColumnName("txn_type");
            entity.Property(e => e.VchDate)
                .UseCollation("NOCASE")
                .HasColumnType("DATETIME")
                .HasColumnName("vch_date");
            entity.Property(e => e.VchNo).HasColumnName("vch_no");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc002s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Acc00200>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc002_00");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");

            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00200).HasForeignKey<Acc00200>(d => d.VchId);
        });

        modelBuilder.Entity<Acc00201>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("acc002_01");

            entity.Property(e => e.DetlId).HasColumnName("detl_id");
            entity.Property(e => e.AcDate).HasColumnName("ac_date");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CostId).HasColumnName("cost_id");
            entity.Property(e => e.Nar).UseCollation("NOCASE");
            entity.Property(e => e.RecAmt)
                .HasColumnType("NUMERIC")
                .HasColumnName("rec_amt");
            entity.Property(e => e.TdsAmt)
                .HasColumnType("NUMERIC")
                .HasColumnName("tds_amt");
            entity.Property(e => e.TdsRate)
                .HasColumnType("NUMERIC")
                .HasColumnName("tds_rate");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc00201s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //entity.HasOne(d => d.Vch).WithMany(p => p.Acc00201s)
            //    .HasForeignKey(d => d.VchId)
            //    .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00201).HasForeignKey<Acc00201>(d => d.VchId);
        });

        modelBuilder.Entity<Acc003>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc003");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Against)
                .HasDefaultValue(0)
                .HasColumnName("against");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMERIC")
                .HasColumnName("amount");
            entity.Property(e => e.AppFormNo).HasColumnName("app_form_no");
    
            entity.Property(e => e.ChallanNo)
                .UseCollation("NOCASE")
                .HasColumnName("challan_no");
            entity.Property(e => e.DivId)
                .UseCollation("NOCASE")
                .HasColumnName("div_id");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.Nar).UseCollation("NOCASE");
            entity.Property(e => e.Postdatedid).HasColumnName("postdatedid");
            entity.Property(e => e.RefDate).HasColumnName("ref_date");
            entity.Property(e => e.RefNo)
                .UseCollation("NOCASE")
                .HasColumnName("ref_no");
            entity.Property(e => e.TxnBywhome)
                .UseCollation("NOCASE")
                .HasColumnName("txn_bywhome");
            entity.Property(e => e.TxnDate).HasColumnName("txn_date");
            entity.Property(e => e.TxnDrawnon)
                .UseCollation("NOCASE")
                .HasColumnName("txn_drawnon");
            entity.Property(e => e.TxnNo)
                .UseCollation("NOCASE")
                .HasColumnType("NUMERIC")
                .HasColumnName("txn_no");
            entity.Property(e => e.TxnType).HasColumnName("txn_type");
            entity.Property(e => e.VchDate)
                .UseCollation("NOCASE")
                .HasColumnType("DATETIME")
                .HasColumnName("vch_date");
            entity.Property(e => e.VchNo).HasColumnName("vch_no");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc003s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Motormemo).WithMany(p => p.Acc003s).HasForeignKey(d => d.LrId);

        });

        modelBuilder.Entity<Acc00300>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc003_00");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");

            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00300).HasForeignKey<Acc00300>(d => d.VchId);
        });

        modelBuilder.Entity<Acc00301>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("acc003_01");

            entity.Property(e => e.DetlId).HasColumnName("detl_id");
            entity.Property(e => e.AcDate).HasColumnName("ac_date");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CostId).HasColumnName("cost_id");
            entity.Property(e => e.RecAmt).HasColumnName("rec_amt");
            entity.Property(e => e.TdsAmt).HasColumnName("tds_amt");
            entity.Property(e => e.TdsRate).HasColumnName("tds_rate");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc00301s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
             
            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00301).HasForeignKey<Acc00301>(d => d.VchId);
        });
   
   
        modelBuilder.Entity<Acc005>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc005");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.AcDate).HasColumnName("ac_date");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Against).HasColumnName("against");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMERIC")
                .HasColumnName("amount");
    
            entity.Property(e => e.ChallanNo)
                .UseCollation("NOCASE")
                .HasColumnType("NUMERIC")
                .HasColumnName("challan_no");
            entity.Property(e => e.DivId)
                .UseCollation("NOCASE")
                .HasColumnType("NUMERIC")
                .HasColumnName("div_id");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.InvType)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC")
                .HasColumnName("inv_type");
            entity.Property(e => e.Nar).UseCollation("NOCASE");
            entity.Property(e => e.TransType)
                .HasDefaultValueSql("0")
                .HasColumnName("trans_type");
            entity.Property(e => e.VchDate)
                .UseCollation("NOCASE")
                .HasColumnName("vch_date");
            entity.Property(e => e.VchNo).HasColumnName("vch_no");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc005s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Acc00500>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc005_00");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");

            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00500).HasForeignKey<Acc00500>(d => d.VchId);
        });

        modelBuilder.Entity<Acc00501>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("acc005_01");

            entity.Property(e => e.DetlId).HasColumnName("detl_id");
            entity.Property(e => e.AcDt).HasColumnName("ac_dt");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Amount).HasColumnType("NUMERIC");
            entity.Property(e => e.CostId).HasColumnName("cost_id");
            entity.Property(e => e.Nar).UseCollation("NOCASE");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc00501s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Vch).WithMany(p => p.Acc00501s).HasForeignKey(d => d.VchId);
        });

        modelBuilder.Entity<Acc00507>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc005_07");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.ApprovedBy).UseCollation("NOCASE");

            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00507).HasForeignKey<Acc00507>(d => d.VchId);
        });

        modelBuilder.Entity<Acc006>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc006");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.AcDate).HasColumnName("ac_date");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMERIC")
                .HasColumnName("amount");
 
            entity.Property(e => e.Cashier)
                .UseCollation("NOCASE")
                .HasColumnName("cashier");
            entity.Property(e => e.ChallanNo)
                .UseCollation("NOCASE")
                .HasColumnName("challan_no");
            entity.Property(e => e.DivId)
                .UseCollation("NOCASE")
                .HasColumnName("div_id");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.Nar).UseCollation("NOCASE");
            entity.Property(e => e.TransType).HasColumnName("trans_type");
            entity.Property(e => e.VchDate)
                .UseCollation("NOCASE")
                .HasColumnName("vch_date");
            entity.Property(e => e.VchNo).HasColumnName("vch_no");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc006s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Acc00600>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc006_00");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");

            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00600).HasForeignKey<Acc00600>(d => d.VchId);
        });

        modelBuilder.Entity<Acc00601>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("acc006_01");

            entity.Property(e => e.DetlId).HasColumnName("detl_id");
            entity.Property(e => e.AcDate).HasColumnName("ac_date");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Amount).HasColumnType("NUMERIC");
            entity.Property(e => e.Nar).UseCollation("NOCASE");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc00601s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Vch).WithMany(p => p.Acc00601s).HasForeignKey(d => d.VchId);
        });

        modelBuilder.Entity<Acc00607>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc006_07");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.ApprovedBy).UseCollation("NOCASE");

            entity.HasOne(d => d.Vch).WithOne(p => p.Acc00607).HasForeignKey<Acc00607>(d => d.VchId);
        });

        modelBuilder.Entity<Acc999>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("acc999");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Approval).HasColumnType("bit");
            entity.Property(e => e.BranchId)
                .UseCollation("NOCASE")
                .HasColumnType("char(3)")
                .HasColumnName("branch_id");
            entity.Property(e => e.ChallanId).HasColumnName("challan_id");
            entity.Property(e => e.ChallanNo)
                .UseCollation("NOCASE")
                .HasColumnType("varchar(15)")
                .HasColumnName("challan_no");
            entity.Property(e => e.DivId)
                .UseCollation("NOCASE")
                .HasColumnType("varchar(10)")
                .HasColumnName("div_id");
            entity.Property(e => e.DueDate)
                .UseCollation("NOCASE")
                .HasColumnType("datetime")
                .HasColumnName("due_date");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.Inventory)
                .UseCollation("NOCASE")
                .HasColumnType("varchar")
                .HasColumnName("inventory");
            entity.Property(e => e.Linkedchallan)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar")
                .HasColumnName("linkedchallan");
            entity.Property(e => e.Nar)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar");
            entity.Property(e => e.SupplierCode).HasColumnName("supplier_code");
            entity.Property(e => e.VchDate)
                .UseCollation("NOCASE")
                .HasColumnType("datetime")
                .HasColumnName("vch_date");
            entity.Property(e => e.VchNo).HasColumnName("vch_no");
            entity.Property(e => e.VchType)
                .HasColumnType("smallint")
                .HasColumnName("vch_type");
        });

        modelBuilder.Entity<Acc99901>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("acc999_01");

            entity.Property(e => e.DetlId).HasColumnName("detl_id");
            entity.Property(e => e.AcDate)
                .HasColumnType("datetime")
                .HasColumnName("ac_date");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Against)
                .UseCollation("NOCASE")
                .HasColumnType("varchar")
                .HasColumnName("against");
            entity.Property(e => e.Cramt)
                .HasColumnType("numeric")
                .HasColumnName("cramt");
            entity.Property(e => e.Dramt)
                .HasColumnType("numeric")
                .HasColumnName("dramt");
            entity.Property(e => e.Nar)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(4000)");
            entity.Property(e => e.ProdCode).HasColumnName("prod_code");
            entity.Property(e => e.RefCode).HasColumnName("ref_code");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Acc99901s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Vch).WithMany(p => p.Acc99901s).HasForeignKey(d => d.VchId);
        });

        modelBuilder.Entity<Bilty>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("bilty");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.BillNo).HasColumnName("bill_no");
            entity.Property(e => e.BillDate).HasColumnName("bill_dt");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.DivId).HasColumnName("div_id");
            entity.Property(e => e.From_Dstn).HasColumnName("from_dstn");
            entity.Property(e => e.BiltyNo).HasColumnType("NUMERIC").HasColumnName("bilty_no");
            entity.Property(e => e.To_Dstn).HasColumnName("to_dstn");
            entity.Property(e => e.TotalFreight).HasColumnName("TotalFreight");
          
        });

        modelBuilder.Entity<BiltyAudit>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("bilty_audit");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.CreatedDt).HasColumnName("CreatedDate");
            entity.Property(e => e.ModifiedDt).HasColumnName("ModifiedDate");

            entity.HasOne(d => d.Bilty).WithOne(p => p.BiltyAudit).HasForeignKey<BiltyAudit>(d => d.VchId);
        });

        modelBuilder.Entity<BiltyCommodity>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("bilty_commodity");

            entity.Property(e => e.DetlId).HasColumnName("Detl_Id");
            entity.Property(e => e.ActWeight)
                .HasColumnType("NUMERIC")
                .HasColumnName("actWeight");
            entity.Property(e => e.ChrgWeight)
                .HasColumnType("NUMERIC")
                .HasColumnName("chrgWeight");
            entity.Property(e => e.Commodity).HasColumnName("commodity");
            entity.Property(e => e.Freight)
                .HasColumnType("NUMERIC")
                .HasColumnName("freight");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMERIC")
                .HasColumnName("qty");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMERIC")
                .HasColumnName("rate");
            entity.Property(e => e.Uom).HasColumnName("uom");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.Bilty).WithMany(p => p.BiltyCommodities).HasForeignKey(d => d.VchId);
        });


        modelBuilder.Entity<BiltyDetail>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("bilty_details");

            entity.Property(e => e.DetlId).HasColumnName("Detl_Id");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.Property(e => e.SenderGstin).HasColumnName("SenderGstin");
            entity.Property(e => e.SenderMobileNo).HasColumnType("NUMERIC").HasColumnName("SenderMobileNo");
            entity.Property(e => e.SenderName).HasColumnName("SenderName");
            entity.Property(e => e.SenderAddress).HasColumnName("SenderAddress");
            entity.Property(e => e.SenderPin).HasColumnType("NUMERIC").HasColumnName("SenderPin");
            entity.Property(e => e.SenderStateId).HasColumnType("NUMERIC").HasColumnName("SenderStateId");
            entity.Property(e => e.SenderMail).HasColumnName("SenderMail");
            entity.Property(e => e.SenderBillNo).HasColumnType("NUMERIC").HasColumnName("SenderBillNo");
            entity.Property(e => e.SenderBillDt).HasColumnName("SenderBillDt");
            entity.Property(e => e.ReceiverGstin).HasColumnName("ReceiverGstin");
            entity.Property(e => e.ReceiverMobileNo).HasColumnType("NUMERIC").HasColumnName("ReceiverMobileNo");
            entity.Property(e => e.ReceiverName).HasColumnName("ReceiverName");
            entity.Property(e => e.ReceiverAddress).HasColumnName("ReceiverAddress");
            entity.Property(e => e.ReceiverPin).HasColumnType("NUMERIC").HasColumnName("ReceiverPin");
            entity.Property(e => e.ReceiverStateId).HasColumnType("NUMERIC").HasColumnName("ReceiverStateId");
            entity.Property(e => e.ReceiverMail).HasColumnName("ReceiverMail");
            entity.Property(e => e.EwayNo).HasColumnType("NUMERIC").HasColumnName("EwayNo");

            entity.HasOne(d => d.Bilty).WithOne(p => p.BiltyDetails).HasForeignKey<BiltyDetail>(d => d.VchId);
        });

        modelBuilder.Entity<BiltyGstDetails>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("bilty_Gst");
            entity.Property(e => e.DetlId).HasColumnName("detl_id").ValueGeneratedOnAdd();
            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.EffDate).HasColumnName("EffDate");
            entity.Property(e => e.Igst).HasColumnName("igst");
            entity.Property(e => e.Cgst).HasColumnName("cgst");
            entity.Property(e => e.Sgst).HasColumnName("sgst");
            entity.Property(e => e.Cess).HasColumnName("cess");
            entity.Property(e => e.GstAmt).HasColumnName("GstAmt");
            entity.Property(e => e.TotalAmt).HasColumnName("TotalAmt");
            entity.Property(e => e.IgstAmt).HasColumnName("IgstAmt");
            entity.Property(e => e.CgstAmt).HasColumnName("CgstAmt");
            entity.Property(e => e.SgstAmt).HasColumnName("SgstAmt");
            entity.Property(e => e.CessAmt).HasColumnName("CessAmt");

            entity.HasOne(d => d.Bilty).WithOne(p => p.BiltyGstDetails).HasForeignKey<BiltyGstDetails>(d => d.VchId);
        });

            modelBuilder.Entity<Motormemo>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("motormemo");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.Dt).HasColumnName("dt");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.DivId).HasColumnName("div_id");

            entity.Property(e => e.From_Dstn).HasColumnName("from_dstn");
            entity.Property(e => e.MemoNo).HasColumnType("NUMERIC").HasColumnName("memo_No"); 
            entity.Property(e => e.To_Dstn).HasColumnName("to_dstn");
            entity.Property(e => e.VehicleNo).HasColumnName("vehicle_no");
            entity.Property(e => e.TotalFreight).HasColumnType("NUMERIC").HasColumnName("TotalFreight");
            entity.Property(e => e.AdvAmount).HasColumnType("NUMERIC").HasColumnName("AdvAmount");
            entity.Property(e => e.LeftAmount).HasColumnType("NUMERIC").HasColumnName("LeftAmount");
            entity.Property(e => e.FreightType).HasColumnType("NUMERIC").HasColumnName("FreightType");
            entity.Property(e => e.BillAmt).HasColumnType("NUMERIC").HasColumnName("BillAmt");

        });

        modelBuilder.Entity<MotormemoAudit>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("motormemo_audit");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");


            entity.HasOne(d => d.Vch).WithOne(p => p.MotormemoAudit).HasForeignKey<MotormemoAudit>(d => d.VchId);
        });

        modelBuilder.Entity<MotormemoOtherCharges>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("motormemo_OtherCharges");

            entity.Property(e => e.DetlId).HasColumnName("Detl_Id");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

           
            entity.Property(e => e.otherchag).HasColumnType("NUMERIC").HasColumnName("otherchag");
            entity.Property(e => e.S_Id).HasColumnName("s_id");

            entity.Property(e => e.AccCode).HasColumnName("acc_code");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.MotormemoOtherCharges)
              .HasForeignKey(d => d.AccCode)
              .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Sundries).WithMany(p => p.MotormemoOtherCharges)
             .HasForeignKey(d => d.S_Id)
             .OnDelete(DeleteBehavior.ClientSetNull);


            entity.HasOne(d => d.Vch).WithMany(p => p.MotormemoOtherCharges).HasForeignKey(d => d.VchId);
        });

        modelBuilder.Entity<MotormemoCommodity>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("motormemo_commodity");

            entity.Property(e => e.DetlId).HasColumnName("Detl_Id");
            entity.Property(e => e.ActWeight)
                .HasColumnType("NUMERIC")
                .HasColumnName("actWeight");
            entity.Property(e => e.ChrgWeight)
                .HasColumnType("NUMERIC")
                .HasColumnName("chrgWeight");
            entity.Property(e => e.Commodity).HasColumnName("commodity");
            entity.Property(e => e.Freight)
                .HasColumnType("NUMERIC")
                .HasColumnName("freight");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMERIC")
                .HasColumnName("qty");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMERIC")
                .HasColumnName("rate");
            entity.Property(e => e.Uom).HasColumnName("uom");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.Vch).WithMany(p => p.MotormemoCommodities).HasForeignKey(d => d.VchId);
        });

        modelBuilder.Entity<MotormemoDetail>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("motormemo_details");

            entity.Property(e => e.DetlId).HasColumnName("Detl_Id");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.Property(e => e.SenderGstin).HasColumnName("SenderGstin");
            entity.Property(e => e.SenderMobileNo).HasColumnType("NUMERIC").HasColumnName("SenderMobileNo");
            entity.Property(e => e.SenderName).HasColumnName("SenderName");
            entity.Property(e => e.SenderAddress1).HasColumnName("SenderAddress1");
            entity.Property(e => e.SenderPin).HasColumnType("NUMERIC").HasColumnName("SenderPin");
            entity.Property(e => e.SenderStateId).HasColumnType("NUMERIC").HasColumnName("SenderStateId");
            entity.Property(e => e.SenderMail).HasColumnName("SenderMail");
            entity.Property(e => e.SenderBillNo).HasColumnType("NUMERIC").HasColumnName("SenderBillNo");
            entity.Property(e => e.SenderBillDt).HasColumnName("SenderBillDt");
            entity.Property(e => e.ReceiverGstin).HasColumnName("ReceiverGstin");
            entity.Property(e => e.ReceiverMobileNo).HasColumnType("NUMERIC").HasColumnName("ReceiverMobileNo");
            entity.Property(e => e.ReceiverName).HasColumnName("ReceiverName");
            entity.Property(e => e.ReceiverAddress).HasColumnName("ReceiverAddress");
            entity.Property(e => e.ReceiverPin).HasColumnType("NUMERIC").HasColumnName("ReceiverPin");
            entity.Property(e => e.ReceiverStateId).HasColumnType("NUMERIC").HasColumnName("ReceiverStateId");
            entity.Property(e => e.ReceiverMail).HasColumnName("ReceiverMail");
            entity.Property(e => e.EwayNo).HasColumnType("NUMERIC").HasColumnName("EwayNo");

            entity.Property(e => e.senderAccount).HasColumnName("senderAccount");
            entity.Property(e => e.senderAmount).HasColumnType("NUMERIC").HasColumnName("senderAmount");
            entity.Property(e => e.receiverAccount).HasColumnName("receiverAccount");
            entity.Property(e => e.receiverAmount).HasColumnType("NUMERIC").HasColumnName("receiverAmount");

            entity.Property(e => e.ownerAccount).HasColumnName("ownerAccount");
           

            entity.HasOne(d => d.Vch).WithOne(p => p.MotormemoDetails).HasForeignKey<MotormemoDetail>(d => d.VchId);
        });

        modelBuilder.Entity<MotormemoExpense>(entity =>
        {
            entity.HasKey(e => e.DetlId);

            entity.ToTable("motormemo_expense");

            entity.Property(e => e.DetlId).HasColumnName("Detl_Id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Charges)
                .HasColumnType("NUMERIC")
                .HasColumnName("charges");

            entity.Property(e => e.S_Id).HasColumnName("s_id");
            entity.Property(e => e.VchId).HasColumnName("vch_id");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.MotormemoExpenses)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
             
            entity.HasOne(d => d.Vch).WithMany(p => p.MotormemoExpenses).HasForeignKey(d => d.VchId);
            entity.HasOne(d => d.Sundries).WithMany(p => p.MotormemoExpenses).HasForeignKey(d => d.S_Id);
        });

        modelBuilder.Entity<Mst001>(entity =>
        {
            entity.HasKey(e => e.MgCode);

            entity.ToTable("mst001");

            entity.Property(e => e.MgCode).HasColumnName("mg_code");
            entity.Property(e => e.MgAlias).HasColumnName("mg_alias");
            entity.Property(e => e.MgBs).HasColumnName("mg_bs");
            entity.Property(e => e.MgHead).HasColumnName("mg_head");
            entity.Property(e => e.MgName).HasColumnName("mg_name");
            entity.Property(e => e.MgType).HasColumnName("mg_type");
        });

        modelBuilder.Entity<Mst002>(entity =>
        {
            entity.HasKey(e => e.GrpCode);

            entity.ToTable("mst002");

            entity.HasIndex(e => e.GrpName, "IX_mst002_grp_name").IsUnique();

            entity.Property(e => e.GrpCode).HasColumnName("grp_code");
            entity.Property(e => e.GrpName).HasColumnName("grp_name");
            entity.Property(e => e.MgCode).HasColumnName("mg_code");
            entity.Property(e => e.ModifiedDt).HasColumnType("INTEGER");
            entity.Property(e => e.SrNo).HasColumnName("Sr_no");

            entity.HasOne(d => d.MgCodeNavigation).WithMany(p => p.Mst002s)
                .HasForeignKey(d => d.MgCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mst003>(entity =>
        {
            entity.HasKey(e => e.SgCode);

            entity.ToTable("mst003");

            entity.HasIndex(e => e.SgName, "IX_mst003_sg_name").IsUnique();

            entity.Property(e => e.SgCode).HasColumnName("sg_code");
            entity.Property(e => e.GrpCode).HasColumnName("grp_code");
            entity.Property(e => e.SgName).HasColumnName("sg_name");
            entity.Property(e => e.Show).HasColumnName("show");
            entity.Property(e => e.SrNo).HasColumnName("sr_no");

            entity.HasOne(d => d.GrpCodeNavigation).WithMany(p => p.Mst003s)
                .HasForeignKey(d => d.GrpCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mst006>(entity =>
        {
            entity.HasKey(e => e.CityId);

            entity.ToTable("mst006");

            entity.HasIndex(e => new { e.CityName, e.TalukaId }, "IX_mst006_City_name_Taluka_id").IsUnique();

            entity.Property(e => e.CityId).HasColumnName("City_id");
            entity.Property(e => e.CityName)
                .UseCollation("NOCASE")
                .HasColumnName("City_name");
            entity.Property(e => e.CityPin).HasColumnName("City_pin");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.TalukaId).HasColumnName("Taluka_id");

            entity.HasOne(d => d.Taluka).WithMany(p => p.Mst006s)
                .HasForeignKey(d => d.TalukaId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mst00601>(entity =>
        {
            entity.HasKey(e => e.TalukaId);

            entity.ToTable("mst006_01");

            entity.HasIndex(e => new { e.TalukaName, e.DistrictId }, "IX_mst006_01_Taluka_name_District_id").IsUnique();

            entity.Property(e => e.TalukaId).HasColumnName("Taluka_id");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.DistrictId).HasColumnName("District_id");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.TalukaName)
                .UseCollation("NOCASE")
                .HasColumnName("Taluka_name");

            entity.HasOne(d => d.District).WithMany(p => p.Mst00601s)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mst00602>(entity =>
        {
            entity.HasKey(e => e.DistrictId);

            entity.ToTable("mst006_02");

            entity.HasIndex(e => new { e.DistrictName, e.StateCode }, "IX_mst006_02_District_name_State_Code").IsUnique();

            entity.Property(e => e.DistrictId).HasColumnName("District_id");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.DistrictName)
                .UseCollation("NOCASE")
                .HasColumnName("District_name");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.StateCode).HasColumnName("State_Code");

            entity.HasOne(d => d.StateCodeNavigation).WithMany(p => p.Mst00602s)
                .HasForeignKey(d => d.StateCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mst00603>(entity =>
        {
            entity.HasKey(e => e.StateCode);

            entity.ToTable("mst006_03");

            entity.Property(e => e.StateCode)
                .ValueGeneratedNever()
                .HasColumnName("State_Code");
            entity.Property(e => e.StateName).HasColumnName("State_name");
            entity.Property(e => e.StateUt).HasColumnName("State_UT");
            entity.Property(e => e.VehicleSeries).HasColumnName("vehicleSeries");
        });

        modelBuilder.Entity<Mst010>(entity =>
        {
            entity.HasKey(e => e.IId);

            entity.ToTable("mst010");

            entity.HasIndex(e => e.IName, "IX_mst010_I_Name").IsUnique();

            entity.Property(e => e.IId).HasColumnName("I_id");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.IGst)
                .HasColumnType("NUMERIC")
                .HasColumnName("I_Gst");
            entity.Property(e => e.IHsn)
                .UseCollation("NOCASE")
                .HasColumnName("I_HSN");
            entity.Property(e => e.IHsnDescription)
                .UseCollation("NOCASE")
                .HasColumnName("I_HsnDescription");
            entity.Property(e => e.IName)
                .UseCollation("NOCASE")
                .HasColumnName("I_Name");
            entity.Property(e => e.IUnit)
                .UseCollation("NOCASE")
                .HasColumnName("I_Unit");
            entity.Property(e => e.ModifiedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");

            entity.HasOne(d => d.IUnitNavigation).WithMany(p => p.Mst010s).HasForeignKey(d => d.IUnit);
        });

        modelBuilder.Entity<Mst011>(entity =>
        {
            entity.HasKey(e => e.AccCode);

            entity.ToTable("mst011");

            entity.HasIndex(e => new { e.AccName, e.PlaceId }, "IX_mst011_Acc_name_place_id").IsUnique();

            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.AccAlias)
                .UseCollation("NOCASE")
                .HasColumnName("Acc_alias");
            entity.Property(e => e.AccName)
                .UseCollation("NOCASE")
                .HasColumnName("Acc_name");
            entity.Property(e => e.CinNo).HasColumnName("cin_no");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.PanNo)
                .HasDefaultValueSql("0")
                .HasColumnName("Pan_No");
            entity.Property(e => e.PlaceId).HasColumnName("place_id");
            entity.Property(e => e.SgCode).HasColumnName("sg_code");
            entity.Property(e => e.TanNo).HasColumnName("Tan_no");
            entity.Property(e => e.IsDisabled).HasColumnName("Is_Disabled");

            entity.HasOne(d => d.Place)
            .WithMany(p => p.Mst011s)
            .HasForeignKey(d => d.PlaceId);
           

            entity.HasOne(d => d.SgCodeNavigation).WithMany(p => p.Mst011s)
                .HasForeignKey(d => d.SgCode)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Mst01100>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("mst011_00");
             

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
       
            entity.Property(e => e.Crbal)
                .HasColumnType("NUMERIC")
                .HasColumnName("crbal");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.DivId)
                .UseCollation("NOCASE")
                .HasColumnName("div_id");
            entity.Property(e => e.Drbal)
                .HasColumnType("NUMERIC")
                .HasColumnName("drbal");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
        

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Mst01100s).HasForeignKey(d => d.AccCode);
        });

        modelBuilder.Entity<Mst01101>(entity =>
        {
            entity.HasKey(e => e.AccCode);

            entity.ToTable("mst011_01");

            entity.Property(e => e.AccCode)
                .ValueGeneratedNever()
                .HasColumnName("acc_code");
            entity.Property(e => e.AccAddress)
                .UseCollation("NOCASE")
                .HasColumnName("acc_address");
            entity.Property(e => e.ContactDesignation)
                .UseCollation("NOCASE")
                .HasColumnName("contact_designation");
            entity.Property(e => e.ContactMobileNo)
                .UseCollation("NOCASE")
                .HasColumnType("NUMERIC")
                .HasColumnName("contact_mobile_no");
            entity.Property(e => e.ContactPerson)
                .UseCollation("NOCASE")
                .HasColumnName("contact_person");
            entity.Property(e => e.EmailId)
                .UseCollation("NOCASE")
                .HasColumnName("email_id");
            entity.Property(e => e.LandlineNo)
                .HasColumnType("NUMERIC")
                .HasColumnName("Landline_No");
            entity.Property(e => e.Website)
                .UseCollation("NOCASE")
                .HasColumnName("website");

            entity.HasOne(d => d.AccCodeNavigation).WithOne(p => p.Mst01101).HasForeignKey<Mst01101>(d => d.AccCode);
        });

       
        modelBuilder.Entity<Mst01104>(entity =>
        {
            entity.HasKey(e => e.AccCode);

            entity.ToTable("mst011_04");

            entity.Property(e => e.AccCode)
                .ValueGeneratedNever()
                .HasColumnName("acc_code");
            entity.Property(e => e.Address)
                .UseCollation("NOCASE")
                .HasColumnName("address");
            entity.Property(e => e.BankName)
                .UseCollation("NOCASE")
                .HasColumnName("bank_name");
            entity.Property(e => e.BankaccNo)
                .UseCollation("NOCASE")
                .HasColumnName("bankaccNo");
            entity.Property(e => e.IfscCode)
                .UseCollation("NOCASE")
                .HasColumnName("ifsc_code");

            entity.HasOne(d => d.AccCodeNavigation).WithOne(p => p.Mst01104).HasForeignKey<Mst01104>(d => d.AccCode);
        });

        modelBuilder.Entity<Mst01109>(entity =>
        {
            entity.HasKey(e => e.AccCode);

            entity.ToTable("mst011_09");

            entity.Property(e => e.AccCode)
                .ValueGeneratedNever()
                .HasColumnName("acc_code");
            entity.Property(e => e.AccGstn)
                .UseCollation("NOCASE")
                .HasColumnName("acc_gstn");
      
            entity.Property(e => e.GstrDate)
                .UseCollation("NOCASE")
                .HasColumnName("GSTR_date");
            entity.Property(e => e.Gstur).HasColumnName("GSTUR"); 

            entity.HasOne(d => d.AccCodeNavigation).WithOne(p => p.Mst01109).HasForeignKey<Mst01109>(d => d.AccCode);
        });

        modelBuilder.Entity<Mst01110>(entity =>
        {
            entity.HasKey(e => e.id);

            entity.ToTable("mst011_10");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.firmCode).HasColumnName("firm_code");
           

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Mst01110s).HasForeignKey(d => d.AccCode);
        });

        modelBuilder.Entity<Mst01115>(entity =>
        {
            entity.HasKey(e => new { e.DivId, e.AccCode });

            entity.ToTable("mst011_15");

            entity.Property(e => e.DivId).HasColumnName("div_id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.TcsApplicable)
                .HasDefaultValueSql("0")
                .HasColumnName("tcs_applicable");
            entity.Property(e => e.TdsApplicable)
                .HasDefaultValueSql("0")
                .HasColumnName("tds_applicable");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Mst01115s).HasForeignKey(d => d.AccCode);
        });

        modelBuilder.Entity<Mst012>(entity =>
        {
            entity.HasKey(e => e.UnitCode);

            entity.ToTable("mst012");

            entity.Property(e => e.UnitCode)
                .UseCollation("NOCASE")
                .HasColumnName("unit_code");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.UnitName)
                .UseCollation("NOCASE")
                .HasColumnName("unit_name");
        });

        modelBuilder.Entity<Mst030>(entity =>
        {

            entity.HasKey(e => e.Id);

            entity.ToTable("mst030");

            entity.Property(e => e.AccCode).HasColumnName("Acc_code");
            

            entity.Property(e => e.Address) .HasColumnName("address")
             .UseCollation("NOCASE");

            entity.Property(e => e.Placeid).HasColumnName("place_id")
             .UseCollation("NOCASE");

            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP")
             .UseCollation("NOCASE");

            entity.Property(e => e.EmailId).HasColumnName("email_id")
             .UseCollation("NOCASE");

            entity.Property(e => e.GstinNo).HasColumnName("gstin_no")
             .UseCollation("NOCASE");

            entity.Property(e => e.MobileNo).HasColumnName("mobile_no")
             .UseCollation("NOCASE");

            entity.Property(e => e.Name).HasColumnName("name")
             .UseCollation("NOCASE");

            entity.Property(e => e.pincode).HasColumnName("pincode")
             .UseCollation("NOCASE");
             

            entity.Property(e => e.StateCode).HasColumnName("State_Code")
             .UseCollation("NOCASE");

            entity.HasOne(d => d.AccCodeNavigation).WithMany()
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.SetNull);
        });


        modelBuilder.Entity<Mst031>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("mst031");

            entity.Property(e => e.VchId).HasColumnName("vch_id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.ECess)
                .HasColumnType("NUMERIC")
                .HasColumnName("e_cess");
            entity.Property(e => e.HeCess)
                .HasColumnType("NUMERIC")
                .HasColumnName("he_cess");
            entity.Property(e => e.SfCess)
                .HasColumnType("NUMERIC")
                .HasColumnName("sf_cess");
            entity.Property(e => e.Tds)
                .HasColumnType("NUMERIC")
                .HasColumnName("tds");
            entity.Property(e => e.TdsName).HasColumnName("tds_name");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Mst031s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mst103>(entity =>
        {
            entity.HasKey(e => e.DeclrId);

            entity.ToTable("mst103");

            entity.Property(e => e.DeclrId).HasColumnName("declr_id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.DeclrNo)
                .UseCollation("NOCASE")
                .HasColumnName("declr_no");
            entity.Property(e => e.FromDt).HasColumnName("from_dt");
            entity.Property(e => e.FyId)
                .UseCollation("NOCASE")
                .HasColumnName("fy_id");
            entity.Property(e => e.Ishuf).HasColumnName("ishuf");
            entity.Property(e => e.NoOfVehicles).HasColumnName("noOfVehicles");
            entity.Property(e => e.PanNo)
                .UseCollation("NOCASE")
                .HasColumnName("pan_no");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Mst103s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mst10300>(entity =>
        {
            entity.HasKey(e => e.VchId);

            entity.ToTable("mst103_00");

            entity.Property(e => e.VchId)
                .ValueGeneratedNever()
                .HasColumnName("vch_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");

            entity.HasOne(d => d.Vch).WithOne(p => p.Mst10300).HasForeignKey<Mst10300>(d => d.VchId);
        });


        modelBuilder.Entity<Mst10301>(entity =>
        {
            entity.HasKey(e => e.detlid);

            entity.ToTable("mst103_01");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.detlid).HasColumnName("detl_id");
            entity.Property(e => e.VehicleNo).HasColumnName("Vehicle_No");

            entity.HasOne(d => d.Vch).WithMany(p => p.Mst10301s).HasForeignKey(d => d.Id); 

        });

        modelBuilder.Entity<Mst107>(entity =>
        {
            entity.HasKey(e => e.VtypeId);

            entity.ToTable("mst107");

            entity.Property(e => e.VtypeId).HasColumnName("Vtype_id");
            entity.Property(e => e.Capacity)
                .HasColumnType("NUMERIC")
                .HasColumnName("capacity");
            entity.Property(e => e.CreatedDt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Vheight)
                .HasColumnType("NUMERIC")
                .HasColumnName("vheight");
            entity.Property(e => e.Vlength)
                .HasColumnType("NUMERIC")
                .HasColumnName("vlength");
            entity.Property(e => e.VtypeName).HasColumnName("Vtype_name");
            entity.Property(e => e.Vwidth)
                .HasColumnType("NUMERIC")
                .HasColumnName("vwidth");
        });

        modelBuilder.Entity<Mst108>(entity =>
        {
            entity.HasKey(e => e.VehicleNo);

            entity.ToTable("mst108");

            entity.Property(e => e.VehicleNo).HasColumnName("Vehicle_no");
            entity.Property(e => e.AccCode).HasColumnName("acc_code"); 
            entity.Property(e => e.CapacityMts).HasColumnType("NUMERIC"); 
            entity.Property(e => e.VtypeId).HasColumnName("Vtype_id");
            entity.Property(e => e.PanNo).HasColumnName("Pan_no");
            entity.Property(e => e.Alias).HasColumnName("Alias");
            entity.Property(e => e.CreditLimitAmt).HasColumnName("Credit_LimitAmt");

            entity.Property(e => e.Enginno).HasColumnName("Enginno");
            entity.Property(e => e.Chassisno).HasColumnName("Chassisno");
            entity.Property(e => e.DriverName).HasColumnName("DriverName");
            entity.Property(e => e.DriverAddress).HasColumnName("DriverAddress");
            entity.Property(e => e.DriverMobileNo).HasColumnName("DriverMobileNo");
            entity.Property(e => e.DriverLicNo).HasColumnName("DriverLicNo");

            entity.Property(e => e.IsOwn).HasColumnName("Is_Own");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Mst108s)
                .HasForeignKey(d => d.AccCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Vtype).WithMany(p => p.Mst108s)
                .HasForeignKey(d => d.VtypeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
 

        modelBuilder.Entity<Mst10801>(entity =>
        {
            entity.HasKey(e => e.VehTaxId);

            entity.ToTable("mst108_01");

            entity.Property(e => e.VehTaxId).HasColumnName("Veh_tax_Id");
            entity.Property(e => e.FitnessNo).HasColumnName("Fitness_no");
            entity.Property(e => e.FitnessFrom).HasColumnName("fitness_from");
            entity.Property(e => e.FitnessTo).HasColumnName("fitness_to");

            entity.Property(e => e.InsuranceNo).HasColumnName("Insurance_no");
            entity.Property(e => e.InsuranceFrom).HasColumnName("Insurance_from");
            entity.Property(e => e.InsuranceTo).HasColumnName("Insurance_to");

            entity.Property(e => e.Permitno).HasColumnName("permitno");
            entity.Property(e => e.PermitFm).HasColumnType("DATE").HasColumnName("permit_fm");
            entity.Property(e => e.PermitTo).HasColumnType("DATE").HasColumnName("permit_to"); 
            entity.Property(e => e.VehicleNo).HasColumnName("Vehicle_no");



            entity.HasOne(d => d.VehicleNoNavigation).WithOne(p => p.Mst10801s).HasForeignKey<Mst10801>(d => d.VehicleNo);
        });
 
        modelBuilder.Entity<Mst10803>(entity =>
        {
            entity.HasKey(e => e.VehicleNo);

            entity.ToTable("mst108_03");

            entity.Property(e => e.VehicleNo).HasColumnName("vehicle_no");
            entity.Property(e => e.BranchId)
                .HasColumnType("NUMERIC")
                .HasColumnName("branch_id");
            entity.Property(e => e.ExpAccCode).HasColumnName("exp_acc_code");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.ProvAccCode).HasColumnName("prov_acc_code");

            entity.HasOne(d => d.ExpAccCodeNavigation).WithMany(p => p.ExpAccCodeNavigations)
                .HasForeignKey(d => d.ExpAccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
 

            entity.HasOne(d => d.VehicleNoNavigation).WithOne(p => p.Mst10803).HasForeignKey<Mst10803>(d => d.VehicleNo);
        });

        modelBuilder.Entity<Mst10804>(entity =>
        {
            entity.HasKey(e => e.VehicleNo);

            entity.ToTable("mst108_04");

            entity.Property(e => e.VehicleNo).HasColumnName("vehicle_no");
            entity.Property(e => e.Address)
                .UseCollation("NOCASE")
                .HasColumnName("address");
            entity.Property(e => e.BankName)
                .UseCollation("NOCASE")
                .HasColumnName("bank_name");
            entity.Property(e => e.BankaccNo)
                .UseCollation("NOCASE")
                .HasColumnName("bankaccNo");
            entity.Property(e => e.IfscCode)
                .UseCollation("NOCASE")
                .HasColumnName("ifsc_code");

            entity.HasOne(d => d.VehicleNoNavigation).WithOne(p => p.Mst10804).HasForeignKey<Mst10804>(d => d.VehicleNo);
        });

        modelBuilder.Entity<Mst10805>(entity =>
        {
            entity.HasKey(e => e.VehSId);

            entity.ToTable("mst108_05");

            entity.Property(e => e.VehSId).HasColumnName("Veh_s_Id");
            entity.Property(e => e.PermitFm).HasColumnName("permit_fm");
            entity.Property(e => e.PermitTo).HasColumnName("permit_to");
            entity.Property(e => e.StateId).HasColumnName("State_id");
            entity.Property(e => e.TaxFm).HasColumnName("Tax_fm");
            entity.Property(e => e.TaxTo).HasColumnName("Tax_to");
            entity.Property(e => e.VehicleNo).HasColumnName("Vehicle_no");

            entity.HasOne(d => d.State).WithMany(p => p.Mst10805s)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.VehicleNoNavigation).WithMany(p => p.Mst10805s).HasForeignKey(d => d.VehicleNo);
        });

        modelBuilder.Entity<Mst152>(entity =>
        {
            entity.HasKey(e => e.FtId);

            entity.ToTable("mst152");

            entity.Property(e => e.FtId).HasColumnName("ft_id");
            entity.Property(e => e.DealerCtg).HasColumnName("dealer_ctg");
            entity.Property(e => e.FtName).HasColumnName("ft_name");
        });
         
        modelBuilder.Entity<Sundry>(entity =>
        {
            entity.HasKey(e => e.S_Id);

            entity.ToTable("Sundry");

            entity.Property(e => e.S_Id).HasColumnName("S_Id");
            entity.Property(e => e.ExpaccCode).HasColumnName("Expacc_code");
            entity.Property(e => e.Operation).HasColumnName("operation");
            entity.Property(e => e.SundryName).HasColumnName("sundry_name");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Sundries)
                .HasForeignKey(d => d.ExpaccCode)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Sys001>(entity =>
        {
            entity.ToTable("sys001");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DescKey).HasColumnName("desc_key");
            entity.Property(e => e.DescName)
                .UseCollation("NOCASE")
                .HasColumnName("desc_name");
            entity.Property(e => e.DescValue)
                .UseCollation("NOCASE")
                .HasColumnName("desc_value");
        });

        modelBuilder.Entity<Sys002>(entity =>
        {
            entity.HasKey(e => e.VchType);

            entity.ToTable("sys002");

            entity.Property(e => e.VchType)
                .ValueGeneratedNever()
                .HasColumnName("vch_type");
            entity.Property(e => e.AutoNo)
                .HasDefaultValueSql("1")
                .HasColumnName("autoNo");
            entity.Property(e => e.Description).UseCollation("NOCASE");
            entity.Property(e => e.InclInDayend)
                .HasDefaultValueSql("0")
                .HasColumnName("incl_in_dayend");
            entity.Property(e => e.MsgString).UseCollation("NOCASE");
            entity.Property(e => e.Padding)
                .HasDefaultValueSql("0")
                .HasColumnName("padding");
            entity.Property(e => e.Pattern)
                .UseCollation("NOCASE")
                .HasColumnName("pattern");
            entity.Property(e => e.Prefix)
                .UseCollation("NOCASE")
                .HasColumnName("prefix");
            entity.Property(e => e.SrNo).HasColumnName("sr_no");
            entity.Property(e => e.Variables)
                .UseCollation("NOCASE")
                .HasColumnName("variables");
            entity.Property(e => e.VchLength).HasColumnName("vchLength");
            entity.Property(e => e.VchTypeName)
                .UseCollation("NOCASE")
                .HasColumnName("vch_type_name");
        });

        modelBuilder.Entity<Sys010>(entity =>
        {
            entity.ToTable("sys010");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccCode).HasColumnName("acc_code");
            entity.Property(e => e.Description).UseCollation("NOCASE");

            entity.HasOne(d => d.AccCodeNavigation).WithMany(p => p.Sys010s).HasForeignKey(d => d.AccCode);
        });

        modelBuilder.Entity<Sys200>(entity =>
        {
            entity.ToTable("sys200");

            entity.Property(e => e.BranchId)
                .UseCollation("NOCASE")
                .HasColumnName("branch_id");
            entity.Property(e => e.ChallanNo)
                .UseCollation("NOCASE")
                .HasColumnName("challan_no");
            entity.Property(e => e.EntityName)
                .UseCollation("NOCASE")
                .HasColumnName("entityName");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.NewValue).UseCollation("NOCASE");
            entity.Property(e => e.OldValue).UseCollation("NOCASE");
            entity.Property(e => e.PmKey)
                .UseCollation("NOCASE")
                .HasColumnName("pmKey");
            entity.Property(e => e.PropertyName).UseCollation("NOCASE");
            entity.Property(e => e.Status).UseCollation("NOCASE");
            entity.Property(e => e.UserName).UseCollation("NOCASE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
