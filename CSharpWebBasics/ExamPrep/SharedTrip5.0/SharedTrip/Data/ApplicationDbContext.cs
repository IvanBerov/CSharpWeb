using SharedTrip.Data.Models;

namespace SharedTrip.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTrip>(entity =>
            {
                entity
                    .HasKey(k => new { k.TripId, k.UserId });

                entity
                    .HasOne(ut => ut.User)
                    .WithMany(u => u.UserTrips)
                    .HasForeignKey(ut => ut.UserId);

                entity
                    .HasOne(ut => ut.Trip)
                    .WithMany(t => t.UserTrips)
                    .HasForeignKey(ut => ut.TripId);
            });
        }
    }
}
