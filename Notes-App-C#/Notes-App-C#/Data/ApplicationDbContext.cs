using Microsoft.EntityFrameworkCore;
using Notes_App_C_.Models;

namespace Notes_App_C_.Data
{
    // DŮLEŽITÉ: Musí zde být ": DbContext"
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Tohle je tvůj Connection String přímo v kódu jako pojistka
                optionsBuilder.UseNpgsql("Host=aws-1-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ledestbbswhpgxyzkayi;Password=7456hFdBMk_74;SSL Mode=Require;Trust Server Certificate=true");
            }
        }
    }
}