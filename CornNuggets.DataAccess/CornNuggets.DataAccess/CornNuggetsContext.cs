using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CornNuggets.DataAccess.Models;

namespace CornNuggets.DataAccess
{
    public partial class CornNuggetsContext : DbContext
    {
        public CornNuggetsContext()
        {
        }

        public CornNuggetsContext(DbContextOptions<CornNuggetsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<NuggetStores> NuggetStores { get; set; }
        public virtual DbSet<OrderLog> OrderLog { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__Customer__A4AE64B896EF49BF");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PreferredStore)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('TEXA001')");
            });

            modelBuilder.Entity<NuggetStores>(entity =>
            {
                entity.HasKey(e => e.StoreId)
                    .HasName("PK__NuggetSt__3B82F0E120BEFF47");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreLocation)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Arlington')");

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('TEXA001')");
            });

            modelBuilder.Entity<OrderLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__OrderLog__5E5499A8A224E901");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductQty).HasDefaultValueSql("((1))");

                entity.Property(e => e.SubTotal).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLog)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderLog__OrderI__3DE82FB7");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderLog)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__OrderLog__Produc__3EDC53F0");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Orders__C3905BAF4C4174B1");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DateTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Orders__Customer__3A179ED3");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Orders__StoreID__39237A9A");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Products__B40CC6ED28E3E0DD");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Inventory).HasDefaultValueSql("((1000))");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0.00))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
