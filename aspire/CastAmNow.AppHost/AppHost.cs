using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var dbProvider = builder.Configuration["DB_PROVIDER"]?.ToLower() ?? "sqlserver";

var sqlServer = builder.AddAzureSqlServer("dbserver");
var postgres = builder.AddPostgres("postgrescastamnow", port: 23456)
    .WithPgAdmin()
    .WithDataVolume($"{nameof(CastAmNow_Web)}")
    .WithLifetime(ContainerLifetime.Persistent);
var storage = builder.AddAzureStorage("storage");
var filesContainer = storage.AddBlobContainer("files", blobContainerName: "files");

if (builder.ExecutionContext.IsRunMode)
{
    storage.RunAsEmulator(s =>
    {
        s.WithBlobPort(55907);
        s.WithQueuePort(55908);
        s.WithTablePort(55909);
        s.WithDataVolume();
        s.WithLifetime(ContainerLifetime.Persistent);
    });
    sqlServer.RunAsContainer(s =>
    {
        s.WithHostPort(12345);
        s.WithDataVolume("CastAmNow");
        s.WithLifetime(ContainerLifetime.Persistent);
    });
}

IResourceBuilder<IResourceWithConnectionString> defectDb = dbProvider == "postgresql" 
    ? postgres.AddDatabase("DefectDb") 
    : sqlServer.AddDatabase("DefectDb");

var defectApi = builder.AddProject<Projects.CastAmNow_Defect_API>("api")
    .WithReference(defectDb)
    .WithEnvironment("DB_PROVIDER", dbProvider)
    .WithExternalHttpEndpoints();

var web = builder.AddProject<Projects.CastAmNow_Web>("web")
    .WithReference(filesContainer)
    .WithReference(defectApi)
    .WithExternalHttpEndpoints();

var migrationService = builder
    .AddProject<Projects.CastAmNow_Defect_MigrationService>("migration-service")
    .WithReference(defectDb)
    .WithEnvironment("DB_PROVIDER", dbProvider);

if (dbProvider == "postgresql")
{
    migrationService.WaitFor(postgres);
}
else
{
    migrationService.WaitFor(sqlServer);
}

if (builder.ExecutionContext.IsRunMode)
{
    var seedData = builder.AddProject<Projects.CastAmNow_SeedData>("seeddata")
        .WithReference(defectDb)
        .WithEnvironment("DB_PROVIDER", dbProvider)
        .SeedDatabaseCommand()
        .ResetDatabaseCommand()
        .WithExplicitStart();
}

builder.Build().Run();