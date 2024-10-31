using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Models;

public partial class AuctionHouseDbContext : DbContext
{
    public AuctionHouseDbContext()
    {
    }

    public AuctionHouseDbContext(DbContextOptions<AuctionHouseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=AuctionHouseConnection");
        }
        optionsBuilder.UseLazyLoadingProxies();
    }
        

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bid__3214EC27E0F62B95");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Bids).HasConstraintName("Bid_Fk_Buyer");

            entity.HasOne(d => d.Item).WithMany(p => p.Bids).HasConstraintName("Bid_Fk_Item");
        });

        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Buyer__3214EC27841A254E");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Item__3214EC27266BE5EF");

            entity.HasOne(d => d.Seller).WithMany(p => p.Items).HasConstraintName("Item_Fk_Seller");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seller__3214EC27A6FDB0A4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
