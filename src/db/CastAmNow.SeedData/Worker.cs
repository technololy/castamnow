using Bogus;
using CastAmNow.Defect.Data;
using System.Diagnostics;

namespace CastAmNow.SeedData;

public class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, ILogger<Worker> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Seeding database", ActivityKind.Client);
        logger.LogInformation("Seeding database");
        try
        {
            using var scope = serviceProvider.CreateScope();
            var defectDbContext = scope.ServiceProvider.GetRequiredService<DefectDbContext>();
            var seedDb = configuration.GetValue<bool>("seedDb");
            var resetDb = configuration.GetValue<bool>("resetDb");
            if (resetDb)
            {
                await ResetDb(defectDbContext);
            }
            if (seedDb)
            {
                await SeedData(defectDbContext);
            }

        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, ex.Message);
            activity?.AddException(ex);
        }
        hostApplicationLifetime.StopApplication();
    }

    private async Task SeedData(DefectDbContext defectDbContext)
    {
        Randomizer.Seed = new Random(8675309);
        var defects = GenerateRandomDefects(1000);
        await defectDbContext.Defects.AddRangeAsync(defects);
        await defectDbContext.SaveChangesAsync();
    }

    private List<Domain.Defect.Defect> GenerateRandomDefects(int count)
    {
        var faker = new Faker<Domain.Defect.Defect>()
        .RuleFor(d => d.Title, f => f.Lorem.Sentence(5))
        .RuleFor(d => d.Description, f => f.Lorem.Paragraph(2))
        .RuleFor(d => d.Category, f => f.PickRandom<Domain.Defect.Category>())
        .RuleFor(d => d.Severity, f => f.PickRandom<Domain.Defect.Severity>())
        .RuleFor(d => d.Priority, f => f.PickRandom<Domain.Defect.Priority>())
        .RuleFor(d => d.Status, f => f.PickRandom<Domain.Defect.Status>())
        .RuleFor(d => d.Latitude, f => f.Address.Latitude().ToString())
        .RuleFor(d => d.Longitude, f => f.Address.Longitude().ToString())
        .RuleFor(d => d.Location, f => f.Address.City())
        .RuleFor(d => d.Attachments, f =>
        [
            new Domain.Defect.Attachment
            {
                FileUrl = f.Internet.Avatar(),
                FileType = Domain.Defect.FileType.Image
            }
        ]);
        var defects = faker.Generate(count);
        return defects;
    }

    private static async Task ResetDb(DefectDbContext defectDbContext)
    {
        defectDbContext.Defects.RemoveRange(defectDbContext.Defects.ToList());
        await defectDbContext.SaveChangesAsync();
    }
}
