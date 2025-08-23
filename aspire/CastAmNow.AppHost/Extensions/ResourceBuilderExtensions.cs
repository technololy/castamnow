

namespace Aspire.Hosting;

internal static class ResourceBuilderExtensions
{

    internal static IResourceBuilder<T> ResetDatabaseCommand<T>(this IResourceBuilder<T> builder) where T : ProjectResource
    {
        builder
            .WithCommand(name: "reset-db", displayName: "Reset Database", executeCommand: context =>
            {
                builder.WithEnvironment("resetDb", "true");
                builder.WithEnvironment("seedDb", "false");
                return Task.FromResult(new ExecuteCommandResult { Success = true });
            }, commandOptions: new CommandOptions
            {
                IconName = "ArrowClockwise",
                IconVariant = IconVariant.Regular,
                ConfirmationMessage = "Are you sure you want to reset the database?",
                Description = "This command will clear all the generated test data",
                IsHighlighted = false,
            });
        return builder;
    }

    internal static IResourceBuilder<T> SeedDatabaseCommand<T>(this IResourceBuilder<T> builder) where T : ProjectResource
    {
        builder
            .WithCommand(name: "seed-db", displayName: "Seed Database", executeCommand: async context =>
            {
                builder.WithEnvironment("seedDb", "true");
                builder.WithEnvironment("resetDb", "false");
                return new ExecuteCommandResult { Success = true };
            }, commandOptions: new CommandOptions
            {
                IconName = "DatabaseLightning",
                IconVariant = IconVariant.Regular,
                IsHighlighted = false,
                ConfirmationMessage = "Are you sure you want to seed the database?",
                Description = "This command will seed the database with test data",
                UpdateState = state =>
                {
                    bool isPresent = false;
                    if (builder.Resource.TryGetEnvironmentVariables(out var environmentCallbackAnnotations))
                    {
                        foreach (var item in environmentCallbackAnnotations)
                        {
                        }
                    }
                    return isPresent ? ResourceCommandState.Disabled : ResourceCommandState.Enabled;
                }
            });
        return builder;
    }
}
