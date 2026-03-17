using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CastAmNow.Defect.Data
{
    public class DefectDbContextFactory : IDesignTimeDbContextFactory<DefectDbContext>
    {
        public DefectDbContext CreateDbContext(string[] args)
        {
            var dbProvider = Environment.GetEnvironmentVariable("DB_PROVIDER")?.ToLower() ?? "sqlite";

            var optionsBuilder = new DbContextOptionsBuilder<DefectDbContext>();

            if (dbProvider == "mongodb")
            {
                optionsBuilder.UseMongoDB("mongodb://localhost:27017", "DefectDb");
            }
            else if (dbProvider == "postgresql")
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Database=dummy",
                    x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Postgresql"));
            }
            else if (dbProvider == "sqlserver")
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=dummy",
                    x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.SqlServer"));
            }
            else
            {
                // Default to SQLite
                optionsBuilder.UseSqlite(
                    "Data Source=defect.db",
                    x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Sqlite"));
            }
            return new DefectDbContext(optionsBuilder.Options);
        }
    }
}
