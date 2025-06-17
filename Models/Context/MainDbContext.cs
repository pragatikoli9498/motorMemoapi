using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.MainDbEntities;
 

namespace MotorMemo.Models.Context;

public partial class MainDbContext : DbContext
{
    public MainDbContext()
    {
    }

    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mst004> Mst004s { get; set; }

    public virtual DbSet<Mst00401> Mst00401s { get; set; }
     

    public virtual DbSet<Mst00403> Mst00403s { get; set; }

    public virtual DbSet<Mst00409> Mst00409s { get; set; }

    public virtual DbSet<Mst005> Mst005s { get; set; }

 

    public virtual DbSet<Mst00603> Mst00603s { get; set; }
    public virtual DbSet<Sys00201> Sys00201s { get; set; }

    public virtual DbSet<Sys00202> Sys00202s { get; set; }

    public virtual DbSet<Sys00203> Sys00203s { get; set; }

    public virtual DbSet<Sys00204> Sys00204s { get; set; }

    public virtual DbSet<Sys00205> Sys00205s { get; set; }

    public virtual DbSet<Sys00206> Modules { get; set; }

    public virtual DbSet<Sys00207> ModuleUsers { get; set; }

    public virtual DbSet<Sys003> Sys003s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mst004>(entity =>
        {
            entity.HasKey(e => e.FirmCode);

            entity.ToTable("mst004");

            entity.Property(e => e.FirmCode)
                .ValueGeneratedNever()
                .HasColumnName("firm_code");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC")
                .HasColumnName("active");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.EmailId)
                .UseCollation("NOCASE")
                .HasColumnName("email_id");
            entity.Property(e => e.FirmAddress1)
                .UseCollation("NOCASE")
                .HasColumnName("firm_address1"); 
            entity.Property(e => e.FirmAlias)
                .UseCollation("NOCASE")
                .HasColumnName("firm_alias");
            entity.Property(e => e.FirmBankAccno)
                .UseCollation("NOCASE")
                .HasColumnName("firm_bank_accno");
            entity.Property(e => e.FirmBankIfsc)
                .UseCollation("NOCASE")
                .HasColumnName("firm_bank_ifsc");
            entity.Property(e => e.FirmBankName)
                .UseCollation("NOCASE")
                .HasColumnName("firm_bank_name");
            entity.Property(e => e.TransactionId)
                .HasColumnName("Transaction_id");
            entity.Property(e => e.FirmFno)
                .UseCollation("NOCASE")
                .HasColumnName("firm_fno");
            entity.Property(e => e.FirmLegalName).HasColumnName("firm_legalName");
            entity.Property(e => e.FirmMobNo).HasColumnName("firm_mob_no");
            entity.Property(e => e.FirmName)
                .UseCollation("NOCASE")
                .HasColumnName("firm_name");
            entity.Property(e => e.FirmPan)
                .UseCollation("NOCASE")
                .HasColumnName("firm_pan");
            entity.Property(e => e.FirmPinCode).HasColumnName("firm_pin_code");
            entity.Property(e => e.FirmPlace).HasColumnName("firm_place");
            entity.Property(e => e.FirmStateCode).HasColumnName("firm_state_code"); 
            entity.Property(e => e.Jurisdiction).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");

            entity.Property(e => e.WebAddress)
                .UseCollation("NOCASE")
                .HasColumnName("web_address");

            entity.HasOne(d => d.Mst00603).WithMany(p => p.Mst004s).HasForeignKey(e => e.FirmStateCode);
             
        });

        modelBuilder.Entity<Mst00401>(entity =>
        {
            entity.HasKey(e => e.FirmCode);

            entity.ToTable("mst004_01");

            entity.Property(e => e.FirmCode)
                .ValueGeneratedNever()
                .HasColumnName("firm_code");
            entity.Property(e => e.Logo).HasColumnName("logo");

            entity.HasOne(d => d.FirmCodeNavigation).WithOne(p => p.Mst00401).HasForeignKey<Mst00401>(d => d.FirmCode);
        });

        modelBuilder.Entity<Mst00403>(entity =>
        {
            entity.HasKey(e => e.LicId);

            entity.ToTable("mst004_03");

            entity.Property(e => e.LicId)
                
                .HasColumnName("Lic_Id"); 
            entity.Property(e => e.FirmCode).HasColumnName("firm_code");
            entity.Property(e => e.LicName).HasColumnName("Lic_Name");
            entity.Property(e => e.LicNo).HasColumnName("Lic_No");

            entity.HasOne(d => d.Mst004).WithMany(p => p.Mst00403s).HasForeignKey(d => new { d.FirmCode});
        });

        modelBuilder.Entity<Mst00409>(entity =>
        {
            entity.HasKey(e => e.FirmId);
            
            entity.ToTable("mst004_09");

            entity.Property(e => e.FirmId)
              .ValueGeneratedOnAdd()
              .HasColumnName("firm_id");
          
            entity.Property(e => e.GstFrom).HasColumnName("gst_from");
            entity.Property(e => e.GstNo).HasColumnName("gst_no");
            entity.Property(e => e.GstTyp).HasColumnName("gst_typ");

            //entity.HasOne(d => d.Mst004).WithMany(p => p.Mst00409s).HasForeignKey(d => new { d.FirmId }).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Mst004).WithOne(p => p.Mst00409).HasForeignKey<Mst00409>(d =>  d.FirmId);
        });

        modelBuilder.Entity<Mst005>(entity =>
        {
            entity.HasKey(e => new { e.FirmCode,  e.DivId });

            entity.ToTable("mst005");

            entity.Property(e => e.FirmCode).HasColumnName("firm_code"); 
            entity.Property(e => e.DivId)
                .UseCollation("NOCASE")
                .HasColumnName("div_id");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.Fdt).HasColumnName("fdt");
            entity.Property(e => e.FromDivId)
                .UseCollation("NOCASE")
                .HasColumnName("from_div_id");
            entity.Property(e => e.IsprevYr)
                .HasColumnType("NUMERIC")
                .HasColumnName("isprev_yr");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.Prefix)
                .UseCollation("NOCASE")
                .HasColumnName("prefix");
            entity.Property(e => e.Tdt).HasColumnName("tdt"); 
 
          //entity.HasOne(d => d.Mst004).WithMany(p => p.Mst005s).HasForeignKey(d => new { d.FirmCode });

            // Define the relationship with Mst004
            entity.HasOne(d => d.Mst004)  // Mst005 has one Mst004
                .WithMany(p => p.Mst005s)
                .HasForeignKey(d => new { d.FirmCode })    
                .HasConstraintName("FK_Mst005_Mst004");

        });

      

        modelBuilder.Entity<Mst00603>(entity =>
        {
            entity.HasKey(e => e.StateCode);
            entity.ToTable("mst006_03");
            entity.Property(e => e.StateCode)
                .ValueGeneratedNever()
                .HasColumnName("state_code");
            entity.Property(e => e.CreatedUser).UseCollation("NOCASE");
            entity.Property(e => e.ModifiedUser).UseCollation("NOCASE");
            entity.Property(e => e.StateCapital).HasColumnName("StateCapital");
            entity.Property(e => e.StateName).HasColumnName("state_name");
            entity.Property(e => e.StateUt)
                .HasColumnType("bit")
                .HasColumnName("state_ut");
            entity.Property(e => e.VehicleSeries).HasColumnName("vehicleSeries");
        });

      modelBuilder.Entity<Sys00201>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("sys002_01");

            entity.HasIndex(e => e.UserId, "IX_sys002_01_user_id").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Password).HasColumnName("password");

            entity.HasOne(d => d.User).WithOne(p => p.Sys00201).HasForeignKey<Sys00201>(d => d.UserId);
        });

        modelBuilder.Entity<Sys00202>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("sys002_02");

            entity.HasIndex(e => e.RoleId, "IX_sys002_02_role_id").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<Sys00203>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("sys002_03");

            entity.HasIndex(e => e.UserId, "IX_sys002_03_user_id").IsUnique();

            entity.HasIndex(e => e.UserName, "IX_sys002_03_user_name").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.LocalIp).HasColumnName("LocalIP");
            entity.Property(e => e.Mobileno).HasColumnName("mobileno");
            entity.Property(e => e.Otp).HasColumnName("otp");
            entity.Property(e => e.OtpExpiry).HasColumnName("otpExpiry");
            entity.Property(e => e.PublicIp).HasColumnName("PublicIP");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UserLongname).HasColumnName("userLongname");
            entity.Property(e => e.UserName).HasColumnName("user_name");

            entity.HasOne(d => d.Role).WithMany(p => p.Sys00203s).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<Sys00204>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("sys002_04");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.A)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.D)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.E)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.EAdmin)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMERIC")
                .HasColumnName("eAdmin");
            entity.Property(e => e.J)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.L)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.O)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.P)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.Sysadmin)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");

            entity.HasOne(d => d.User).WithOne(p => p.Sys00204).HasForeignKey<Sys00204>(d => d.UserId);
        });

        modelBuilder.Entity<Sys00205>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("sys002_05");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.A)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.B)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.D)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.E)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.L)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.O)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.P)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.R)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMERIC");

            entity.HasOne(d => d.User).WithOne(p => p.Sys00205).HasForeignKey<Sys00205>(d => d.UserId);
        });

        modelBuilder.Entity<Sys00206>(entity =>
        {
            entity.ToTable("sys002_06");

            entity.HasIndex(e => e.Id, "IX_sys002_06_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Modulename).HasColumnName("modulename");
        });

        modelBuilder.Entity<Sys00207>(entity =>
        {
            entity.ToTable("sys002_07");

            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Module).WithMany(p => p.Sys00207s)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.User).WithMany(p => p.Sys00207s)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Sys003>(entity =>
        {
            entity.HasKey(e => e.EndId);

            entity.ToTable("sys003");

            entity.Property(e => e.EndId).HasColumnName("end_id");
            entity.Property(e => e.BranchCode).HasColumnName("branch_code");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.FirmCode).HasColumnName("firm_code");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
