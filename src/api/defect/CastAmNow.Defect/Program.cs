using CastAmNow.Api.Infrastructure.Extensions;
using CastAmNow.Defect.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

const string UseLocalArgs = "/local";
var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServicesInAssembly<Program>(builder.Configuration);
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CastAmNow API", Version = "v1" });

});

builder.Services.AddAutoMapper(typeof(Program));
var configArgs = builder.Configuration.AsEnumerable().Select(kvp => kvp.Key).ToArray();
var dbProvider = builder.Configuration["DB_PROVIDER"]?.ToLower() ?? "sqlite";

if (dbProvider == "mongodb")
{
    if (args.Any(x => x == UseLocalArgs) || configArgs.Any(x => x == UseLocalArgs))
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
    if (args.Any(x => x == UseLocalArgs) || configArgs.Any(x => x == UseLocalArgs))
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
            opts =>
            {
                opts.UseNpgsql(x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Postgresql"));
                opts.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
                opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
    }
}
else if (dbProvider == "sqlserver")
{
    if (args.Any(x => x == UseLocalArgs) || configArgs.Any(x => x == UseLocalArgs))
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
            opts =>
            {
                opts.UseSqlServer(x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.SqlServer"));
                opts.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
                opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
    }
}
else
{
    // Default to SQLite
    if (args.Any(x => x == UseLocalArgs) || configArgs.Any(x => x == UseLocalArgs))
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
            opts =>
            {
                opts.UseSqlite(x => x.MigrationsAssembly("CastAmNow.Defect.Migrations.Sqlite"));
                opts.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
                opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
    }
}


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
