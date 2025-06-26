using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext : DbContext
{
    
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Entities Configuration
        ///PRODUCT
        modelBuilder.Entity<Product>(eb =>
        {
            eb.ToTable("Products");
            eb.Property(p => p.Sku)
            .HasMaxLength(40);
            eb.HasIndex(p => p.Sku)
            .IsUnique();
            eb.Property(p => p.InternalCode)
                .IsRequired()
                .HasMaxLength(60);
            eb.Property(p => p.Name)
            .HasMaxLength(60)
            .IsRequired();
            eb.Property(p => p.Description)
            .HasMaxLength(300);
            eb.Property(p => p.CurrentUnitPrice)
            .HasPrecision(15, 2)
            .IsRequired();
            eb.Property(p => p.StockQuantity)
            .IsRequired();
        });
        //CUSTOMER
        modelBuilder.Entity<Customer>(eb =>
        {
            eb.ToTable("Customers");
            eb.Property(c => c.Name)
           .HasMaxLength(60)
           .IsRequired();
            eb.Property(c => c.Email)
                .HasMaxLength(50)
                .IsRequired();
            eb.Property(c => c.PhoneNumber)
                .HasMaxLength(10)
                .IsRequired();
        });

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }

}
