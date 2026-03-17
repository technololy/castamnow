using CastAmNow.Defect.Data;
using CastAmNow.Defect.MigrationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MongoDB.Driver;

const string useLocalArgs = "/local";


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var dbProvider = builder.Configuration["DB_PROVIDER"]?.ToLower() ?? "sqlite";

if (dbProvider == "mongodb")
{
    if (args.Any(x => x == useLocalArgs))
    {
        builder.Services.AddDbContext<DefectDbContext>(options =>
        {
            options.UseMongoDB(builder.Configuration.GetConnectionString("Default") ?? "mongodb://localhost:27017", "DefectDb");
        });
    }
    else
    {
        builder.AddMongoDBClient("DefectDb");
        builder.Services.AddDbContext<DefectDbContext>((serviceProvider, options) =>
        {
            options.UseMongoDB(serviceProvider.GetRequiredService<IMongoClient>(), "DefectDb");
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });
    }
}
else if (dbProvider == "postgresql")

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
else if (dbProvider == "sqlserver")
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
else
{
    // Default to SQLite
    if (args.Any(x => x == useLocalArgs))
    {
        builder.Services.AddDbContext<DefectDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("Default"), 
                x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Sqlite"));
        });
    }
    else
    {
        builder.AddSqliteDbContext<DefectDbContext>("DefectDb", configureDbContextOptions:
            opts => {
                opts.UseSqlite(x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Sqlite"));
                opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                opts.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
    }
}



builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

