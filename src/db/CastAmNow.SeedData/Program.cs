using CastAmNow.Defect.Data;
using CastAmNow.SeedData;
using Microsoft.EntityFrameworkCore;
const string UseLocalArgs = "/local";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
var dbProvider = builder.Configuration["DB_PROVIDER"]?.ToLower() ?? "sqlserver";

if (dbProvider == "postgresql")
{
    if (args.Any(x => x == UseLocalArgs))
    {
        builder.Services.AddDbContext<DefectDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default"), 
                x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Postgresql"));
        });
    }
    else
    {
        builder.AddNpgsqlDbContext<DefectDbContext>("DefectDb", configureDbContextOptions:
            opts => {
                opts.UseNpgsql(x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Postgresql"));
                opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
    }
}
else
{
    if (args.Any(x => x == UseLocalArgs))
    {
        builder.Services.AddDbContext<DefectDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), 
                x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.SqlServer"));
        });
    }
    else
    {
        builder.AddSqlServerDbContext<DefectDbContext>("DefectDb", configureDbContextOptions:
            opts => {
                opts.UseSqlServer(x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.SqlServer"));
                opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
    }
}
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
