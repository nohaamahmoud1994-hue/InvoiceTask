using Invoice.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoice.API.Data
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options)
           : base(options)
        {
        }

        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<TaxTotal> TaxTotals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Invoice -> InvoiceLines
            // =========================

            modelBuilder.Entity<Invoices>()
                .HasMany(i => i.InvoiceLines)
                .WithOne(l => l.Invoice)
                .HasForeignKey(l => l.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================
            // Invoice -> TaxTotals
            // =========================

            modelBuilder.Entity<Invoices>()
                .HasMany(i => i.TaxTotals)
                .WithOne(t => t.Invoice)
                .HasForeignKey(t => t.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================
            // Issuer
            // =========================

            modelBuilder.Entity<Invoices>()
      .OwnsOne(i => i.Issuer, issuer =>
      {
          issuer.OwnsOne(i => i.Address, address =>
          {
              address.Property(a => a.BranchId);
              address.Property(a => a.Country);
              address.Property(a => a.Governate);
              address.Property(a => a.RegionCity);
              address.Property(a => a.Street);
              address.Property(a => a.BuildingNumber);
          });
      });


            // =========================
            // Receiver
            // =========================

            modelBuilder.Entity<Invoices>()
                .OwnsOne(i => i.Receiver, receiver =>
                {
                    receiver.OwnsOne(r => r.Address, address =>
                    {
                        address.Property(a => a.Country);
                        address.Property(a => a.Governate);
                        address.Property(a => a.RegionCity);
                        address.Property(a => a.Street);
                        address.Property(a => a.BuildingNumber);
                    });
                });
            // =========================
            // InvoiceLine -> Value
            // =========================

            modelBuilder.Entity<InvoiceLine>()
                .OwnsOne(l => l.UnitValue);


            // =========================
            // InvoiceLine -> Discount
            // =========================

            modelBuilder.Entity<InvoiceLine>()
                .OwnsOne(l => l.Discount);


            // =========================
            // InvoiceLine -> TaxableItems
            // =========================

            modelBuilder.Entity<InvoiceLine>()
                .OwnsMany(l => l.TaxableItems);
        }
    }
}
