using FootballManager.Data.Models;

namespace FootballManager.Data
{
    using Microsoft.EntityFrameworkCore;
    public class FootballManagerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<UserPlayer> UserPlayers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=FootballManager;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPlayer>(entity =>
            {
                entity
                    .HasKey(up => new { up.PlayerId, up.UserId });
                entity
                    .HasOne(up => up.User)
                    .WithMany(u => u.UserPlayers)
                    .HasForeignKey(up => up.UserId);
                entity
                    .HasOne(up => up.Player)
                    .WithMany(p => p.UserPlayers)
                    .HasForeignKey(up => up.PlayerId);
            });

        }
    }
}
