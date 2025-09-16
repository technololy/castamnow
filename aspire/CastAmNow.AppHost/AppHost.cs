var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddAzureSqlServer("dbserver");

var storage = builder.AddAzureStorage("storage");
var filesContainer = storage.AddBlobContainer("files", blobContainerName: "files");
if (builder.ExecutionContext.IsRunMode)
{
    storage.RunAsEmulator(s =>
    {
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
var defectDb = sqlServer.AddDatabase("DefectDb");

var defectApi = builder.AddProject<Projects.CastAmNow_Defect_API>("api")
    .WithReference(defectDb);

var web = builder.AddProject<Projects.CastAmNow_Web>("web")
    .WithReference(filesContainer)
    .WithReference(defectApi);

var migrationService = builder
    .AddProject<Projects.CastAmNow_Defect_MigrationService>("migration-service")
    .WithReference(defectDb)
    .WaitFor(sqlServer);

if (builder.ExecutionContext.IsRunMode)
{
    var seedData = builder.AddProject<Projects.CastAmNow_SeedData>("seeddata")
        .WithReference(defectDb)
        .SeedDatabaseCommand()
        .ResetDatabaseCommand()
        .WithExplicitStart();
}

builder.Build().Run();
