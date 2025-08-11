var builder = DistributedApplication.CreateBuilder(args);

var potholes = builder.AddProject<Projects.CastAmNow_Potholes>("potholes");

var web = builder.AddProject<Projects.CastAmNow_Web>("web")
    .WithReference(potholes);

builder.Build().Run();
