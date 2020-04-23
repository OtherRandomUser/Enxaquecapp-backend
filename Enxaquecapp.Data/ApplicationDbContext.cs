using Enxaquecapp.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enxaquecapp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cause> Causes { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Local> Locals { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Relief> Reliefs { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cause>()
                .HasIndex(c => new
                {
                    c.UserId,
                    c.Description
                })
                .IsUnique();

            modelBuilder.Entity<Local>()
                .HasIndex(l => new
                {
                    l.UserId,
                    l.Description
                })
                .IsUnique();

            modelBuilder.Entity<Medication>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<Relief>()
                .HasIndex(r => new
                {
                    r.UserId,
                    r.Description
                })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}