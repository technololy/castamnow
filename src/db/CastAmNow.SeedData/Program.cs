using CastAmNow.Defect.Data;
using CastAmNow.SeedData;
using Microsoft.EntityFrameworkCore;
const string UseLocalArgs = "/local";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
if (args.Any(x => x == UseLocalArgs))
{
    builder.Services.AddDbContext<DefectDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });
}
else
{
    builder.AddSqlServerDbContext<DefectDbContext>("DefectDb", configureDbContextOptions:
        opts => opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
}
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
