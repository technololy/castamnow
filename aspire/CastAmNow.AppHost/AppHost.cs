using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var dbProvider = builder.Configuration["DB_PROVIDER"]?.ToLower() ?? "sqlite";

var sqlServer = builder.AddAzureSqlServer("dbserver");
var postgres = builder.AddPostgres("postgrescastamnow", port: 23456)
    .WithPgAdmin()
    .WithDataVolume($"{nameof(CastAmNow_Web)}")
    .WithLifetime(ContainerLifetime.Persistent);
var mongo = builder.AddMongoDB("mongodb")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var sqlite = builder.AddSqlite("DefectDb");


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

IResourceBuilder<IResourceWithConnectionString> defectDb = dbProvider switch
{
    "postgresql" => postgres.AddDatabase("DefectDb"),
    "mongodb" => mongo.AddDatabase("DefectDb"),
    "sqlserver" => sqlServer.AddDatabase("DefectDb"),
    _ => sqlite
};


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
else if (dbProvider == "mongodb")
{
    migrationService.WaitFor(mongo);
}
else if (dbProvider == "sqlite")
{
    // SQLite doesn't need to wait for a container
}
else if (dbProvider == "sqlserver")
{
    migrationService.WaitFor(sqlServer);
}
else
{
    // SQLite doesn't need to wait for a container
}



if (builder.ExecutionContext.IsRunMode)
{
    var seedData = builder.AddProject<CastAmNow_SeedData>("seeddata")
        .WithReference(defectDb)
        .WithEnvironment("DB_PROVIDER", dbProvider)
        .SeedDatabaseCommand()
        .ResetDatabaseCommand()
        .WithExplicitStart();
}

builder.Build().Run();