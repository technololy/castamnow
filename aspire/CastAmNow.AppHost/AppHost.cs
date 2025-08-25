using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("dbserver", port: 12345)
    .WithDataVolume("CastAmNow")
    .WithLifetime(ContainerLifetime.Persistent);

var storage = builder.AddAzureStorage("storage");
var blobs = storage.AddBlobs("blobs");
var filesContainer = blobs.AddBlobContainer("files", blobContainerName: "files");
if (builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(s =>
    {
        s.WithDataVolume();
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

var seedData = builder.AddProject<Projects.CastAmNow_SeedData>("seeddata")
    .WithReference(defectDb)
    .SeedDatabaseCommand()
    .ResetDatabaseCommand()
    .WithExplicitStart();

builder.Build().Run();
