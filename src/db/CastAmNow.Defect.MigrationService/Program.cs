using CastAmNow.Defect.Data;
using CastAmNow.Defect.MigrationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

const string useLocalArgs = "/local";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var dbProvider = builder.Configuration["DB_PROVIDER"]?.ToLower() ?? "sqlserver";

if (dbProvider == "postgresql")
{
    if (args.Any(x => x == useLocalArgs))
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
                opts.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
    }
}
else
{
    if (args.Any(x => x == useLocalArgs))
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

