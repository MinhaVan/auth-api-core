using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Auth.Data.Context;

namespace Auth.Data
{
    public class APIContextFactory : IDesignTimeDbContextFactory<APIContext>
    {
        public APIContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<APIContext>();

            var connectionString = "Host=localhost;Port=5432;Database=auth-db;Username=admin;Password=admin";
            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Auth.API"));

            return new APIContext(optionsBuilder.Options);
        }
    }
}
