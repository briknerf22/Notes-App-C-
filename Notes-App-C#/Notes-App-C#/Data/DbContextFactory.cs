using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notes_App_C_.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Tady je tvůj Connection String
            optionsBuilder.UseNpgsql("Host=aws-1-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ledestbbswhpgxyzkayi;Password=7456hFdBMk_74;SSL Mode=Require;Trust Server Certificate=true");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}