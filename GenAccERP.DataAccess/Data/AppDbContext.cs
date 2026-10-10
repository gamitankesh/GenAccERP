using System;
using System.Collections.Generic;
using GenAccERP.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemMaster> ItemMasters { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<PurchaseMaster> PurchaseMasters { get; set; }

    public virtual DbSet<SalesDetail> SalesDetails { get; set; }

    public virtual DbSet<SalesMaster> SalesMasters { get; set; }

    public virtual DbSet<VwItemProfitAndLoss> VwItemProfitAndLosses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure the provider here when options have not been configured by the application's DI
        // This keeps the hard-coded connection string out of the execution path when the DbContext is
        // registered with AddDbContext and a connection string is provided via configuration.
            if (!optionsBuilder.IsConfigured)
        {
            // Fallback connection string when no configuration is available (e.g. design-time scenarios)
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=GenAccERP;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AppUser__1788CCAC5E6604F9");

            entity.ToTable("AppUser");

            entity.HasIndex(e => e.Username, "UQ__AppUser__536C85E47860BA76").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__AppUser__A9D1053404901E15").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_AppUser_CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InverseModifiedByNavigation)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_AppUser_ModifiedBy");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ItemCate__19093A2BC386A1FF");

            entity.ToTable("ItemCategory");

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemCategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemCategory_CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemCategoryModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_ItemCategory_ModifiedBy");
        });

        modelBuilder.Entity<ItemMaster>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__ItemMast__727E83EB4F7BA50B");

            entity.ToTable("ItemMaster");

            entity.HasIndex(e => e.ItemNo, "UQ__ItemMast__727D9FE581E61244").IsUnique();

            entity.Property(e => e.ItemId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("ItemID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemDescription).HasMaxLength(500);
            entity.Property(e => e.ItemName).HasMaxLength(150);
            entity.Property(e => e.ItemNo).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SalesPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.ItemMasters)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_ItemMaster_ItemCategory");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemMaster_CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemMasterModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_ItemMaster_ModifiedBy");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.PurchaseDetailId).HasName("PK__Purchase__88C328D5F4554020");

            entity.ToTable("PurchaseDetail", tb => tb.HasTrigger("TRG_UpdateStock_Purchase"));

            entity.Property(e => e.PurchaseDetailId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("PurchaseDetailID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", false)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetail_ItemMaster");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK_PurchaseDetail_PurchaseMaster");
        });

        modelBuilder.Entity<PurchaseMaster>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BDE4DA25346");

            entity.ToTable("PurchaseMaster");

            entity.Property(e => e.PurchaseId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("PurchaseID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SupplierName).HasMaxLength(150);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchaseMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseMaster_CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchaseMasterModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_PurchaseMaster_ModifiedBy");
        });

        modelBuilder.Entity<SalesDetail>(entity =>
        {
            entity.HasKey(e => e.SalesDetailId).HasName("PK__SalesDet__391C57922F9A8F9D");

            entity.ToTable("SalesDetail", tb => tb.HasTrigger("TRG_ValidateAndProcess_Sale"));

            entity.Property(e => e.SalesDetailId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("SalesDetailID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", false)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesDetail_ItemMaster");

            entity.HasOne(d => d.Sale).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_SalesDetail_SalesMaster");
        });

        modelBuilder.Entity<SalesMaster>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__SalesMas__1EE3C41F44E42E30");

            entity.ToTable("SalesMaster");

            entity.Property(e => e.SaleId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("SaleID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SaleDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesMaster_CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesMasterModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_SalesMaster_ModifiedBy");
        });

        modelBuilder.Entity<VwItemProfitAndLoss>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ItemProfitAndLoss");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.EstimatedProfit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ItemName).HasMaxLength(150);
            entity.Property(e => e.ItemNo).HasMaxLength(50);
            entity.Property(e => e.TotalPurchaseCost).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TotalSalesRevenue).HasColumnType("decimal(38, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
