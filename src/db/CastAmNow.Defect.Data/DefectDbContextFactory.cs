using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CastAmNow.Defect.Data
{
    public class DefectDbContextFactory : IDesignTimeDbContextFactory<DefectDbContext>
    {
        public DefectDbContext CreateDbContext(string[] args)
        {
            var dbProvider = Environment.GetEnvironmentVariable("DB_PROVIDER")?.ToLower() ?? "sqlserver";

            var optionsBuilder = new DbContextOptionsBuilder<DefectDbContext>();

            if (dbProvider == "postgresql")
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Database=dummy",
                    x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Postgresql"));
            }
            else
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=dummy",
                    x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.SqlServer"));
            }

            return new DefectDbContext(optionsBuilder.Options);
        }
    }
}
