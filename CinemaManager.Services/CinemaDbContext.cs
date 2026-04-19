using CinemaManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManager.Services;

public class CinemaDbContext(DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    public DbSet<CinemaHall> Halls => Set<CinemaHall>();
    public DbSet<Screening> Screenings => Set<Screening>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CinemaHall>(e =>
        {
            e.HasKey(h => h.Id);
            e.Property(h => h.Id).ValueGeneratedOnAdd();
            e.Property(h => h.HallType).HasConversion<string>();
        });

        modelBuilder.Entity<Screening>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).ValueGeneratedOnAdd();
            e.Property(s => s.Genre).HasConversion<string>();
            e.HasOne<CinemaHall>().WithMany().HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
