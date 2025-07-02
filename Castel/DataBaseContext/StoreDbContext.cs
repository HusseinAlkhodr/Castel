using Castel.Models.Authentication;
using Castel.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Castel.DataBaseContext
{
    public class StoreDbContext : IdentityDbContext<
    StoreUser,
    StoreRole,
    long,
    IdentityUserClaim<long>,
    UserRole,
    IdentityUserLogin<long>,
    IdentityRoleClaim<long>,
    IdentityUserToken<long>>
    {
        public StoreDbContext(DbContextOptions option) : base(option) { }
        public virtual DbSet<StoreUser> Users { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Pricing> Pricing { get; set; }
        public virtual DbSet<BuyInvoice> PurchaseInvoice { get; set; }
        public virtual DbSet<SaleInvoice> SaleInvoice { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<SaleInvoiceMaster> SaleInvoiceMasters { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ExchangeRate>().HasData(new ExchangeRate
            {
                id = 1,
                exchangeRate = 1000.0
            });

            //Many-To-Many Relationship
            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
                entity.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();

            });
            builder.Entity<StoreUserClaim>(entity =>
            {
                entity.HasOne(e => e.User).WithMany(u => u.UserClaims).HasForeignKey(e => e.UserId).IsRequired();
            });
            //One-To-Many Relationship
            builder.Entity<Division>().HasMany(d => d.Items).WithOne(i => i.Division).HasForeignKey(i => i.DivisionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Vendor>().HasMany(v => v.Items).WithOne(i => i.Vendor).HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasMany(p => p.PriceHistory).WithOne(i => i.Item).HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BuyInvoice>().HasOne(p => p.Item).WithMany(i => i.BuyInvoices).HasForeignKey(p => p.ItemId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<SaleInvoice>().HasOne(s => s.Item).WithMany(i => i.SaleInvoices).HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BuyInvoice>().HasOne(p => p.Division).WithMany(i => i.BuyInvoices)
                .HasForeignKey(p => p.DivisionId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<SaleInvoice>().HasOne(s => s.Division).WithMany(i => i.SaleInvoices).HasForeignKey(s => s.DivisionId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<BuyInvoice>().HasOne(p => p.Vendor).WithMany(i => i.BuyInvoices)
                .HasForeignKey(p => p.VendorId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<SaleInvoice>().HasOne(s => s.Vendor).WithMany(i => i.SaleInvoices)
                .HasForeignKey(s => s.VendorId).OnDelete(DeleteBehavior.NoAction);


            builder.Entity<SaleInvoice>().HasOne(si => si.SaleInvoiceMaster).WithMany(sm => sm.SaleInvoices)
                .HasForeignKey(si => si.SaleInvoiceMasterId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<BuyInvoice>().HasOne(si => si.BuyInvoiceMaster).WithMany(sm => sm.BuyInvoices)
                .HasForeignKey(si => si.BuyInvoiceMasterId).OnDelete(DeleteBehavior.Cascade);
        }

    }
}
