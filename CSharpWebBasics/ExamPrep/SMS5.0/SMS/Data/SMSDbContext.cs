using SMS.Data.Models;

namespace SMS.Data
{
    using Microsoft.EntityFrameworkCore;
    
    // ReSharper disable once InconsistentNaming
    public class SMSDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public SMSDbContext()
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

            modelBuilder.Entity<Cart>()
                .HasOne(u => u.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<User>(u => u.CartId);
        }
    }
}