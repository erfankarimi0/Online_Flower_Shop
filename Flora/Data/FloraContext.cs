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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.ToTable("Buyer");

            entity.Property(e => e.FirstName).HasMaxLength(50);
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
