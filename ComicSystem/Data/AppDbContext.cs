using ComicSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ComicBook> ComicBooks => Set<ComicBook>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalDetail> RentalDetails => Set<RentalDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Rentals)
            .HasForeignKey(r => r.CustomerID);

        modelBuilder.Entity<RentalDetail>()
            .HasOne(rd => rd.Rental)
            .WithMany(r => r.RentalDetails)
            .HasForeignKey(rd => rd.RentalID);

        modelBuilder.Entity<RentalDetail>()
            .HasOne(rd => rd.ComicBook)
            .WithMany(cb => cb.RentalDetails)
            .HasForeignKey(rd => rd.ComicBookID);
    }
}