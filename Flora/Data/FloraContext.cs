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
            entity.Property(e => e.PhoneNumber).IsFixedLength();
        });

        modelBuilder.Entity<MidorderProduct>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.MidorderProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MIDOrderProduct_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.MidorderProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MIDOrderProduct_Product");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.InsertDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PostalCode).IsFixedLength();
            entity.Property(e => e.RecipientPhoneNumber).IsFixedLength();
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Buyer");

            entity.HasOne(d => d.Send).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Send");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Shop");
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPhoto_Product");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasOne(d => d.Shop).WithMany(p => p.Sellers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seller_Shop");
        });

        modelBuilder.Entity<Send>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Shipment");

            entity.Property(e => e.InsertDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ShopPhoto>(entity =>
        {
            entity.HasOne(d => d.Shop).WithMany(p => p.ShopPhotos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopPhoto_Shop");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
