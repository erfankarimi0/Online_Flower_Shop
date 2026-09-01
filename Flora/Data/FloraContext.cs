using System;
using System.Collections.Generic;
using Flora.Models;
using Microsoft.EntityFrameworkCore;

namespace Flora.Data;

public partial class FloraContext : DbContext
{
    public FloraContext()
    {
    }

    public FloraContext(DbContextOptions<FloraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<MidorderProduct> MidorderProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPhoto> ProductPhotos { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<Send> Sends { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopPhoto> ShopPhotos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=var;Initial Catalog=Flora;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.ToTable("Buyer");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastLoginDate).HasPrecision(0);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<MidorderProduct>(entity =>
        {
            entity.ToTable("MIDOrderProduct");

            entity.HasOne(d => d.Order).WithMany(p => p.MidorderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MIDOrderProduct_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.MidorderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MIDOrderProduct_Product");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ExactAddress).HasMaxLength(100);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.OrderStatus).HasMaxLength(50);
            entity.Property(e => e.PlateNumber).HasMaxLength(50);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Province).HasMaxLength(50);
            entity.Property(e => e.RecipientFirstName).HasMaxLength(50);
            entity.Property(e => e.RecipientLastName).HasMaxLength(50);
            entity.Property(e => e.RecipientPhoneNumber)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.SenderName).HasMaxLength(50);
            entity.Property(e => e.SenderText).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UnitNumber).HasMaxLength(50);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Buyer");

            entity.HasOne(d => d.Send).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SendId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Send");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Shop");
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.ToTable("ProductPhoto");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPhoto_Product");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.ToTable("Seller");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Send>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Shipment");

            entity.ToTable("Send");

            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SendMethod).HasMaxLength(50);
            entity.Property(e => e.SendStatus).HasMaxLength(50);
            entity.Property(e => e.Sendprice).HasColumnType("money");
            entity.Property(e => e.TimeFrame).HasMaxLength(50);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.ToTable("Shop");

            entity.HasOne(d => d.Seller).WithMany(p => p.Shops)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shop_Seller");
        });

        modelBuilder.Entity<ShopPhoto>(entity =>
        {
            entity.ToTable("ShopPhoto");

            entity.HasOne(d => d.Shop).WithMany(p => p.ShopPhotos)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopPhoto_Shop");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
