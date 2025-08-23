using CastAmNow.Defect.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace CastAmNow.Defect.MigrationService;

public class Worker(IServiceProvider serviceProvider,
                IHostApplicationLifetime hostApplicationLifetime,
                ILogger<Worker> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);
        logger.LogInformation("Migrating database");
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DefectDbContext>();

            await EnsureDatabaseAsync(dbContext, stoppingToken);
            await RunMigrationAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, ex.Message);
            activity?.AddException(ex);
        }
        hostApplicationLifetime.StopApplication();
    }

    private async Task RunMigrationAsync(DefectDbContext dbContext, CancellationToken stoppingToken)
    {
        logger.LogInformation("Applying migration to database");
        await dbContext.Database.MigrateAsync(stoppingToken);
    }

    private async Task EnsureDatabaseAsync(DefectDbContext dbContext, CancellationToken stoppingToken)
    {
        logger.LogInformation("Setting up database");
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(stoppingToken))
            {
                await dbCreator.CreateAsync(stoppingToken);
            }
        });
    }
}
