var builder = DistributedApplication.CreateBuilder(args);

var defects = builder.AddProject<Projects.CastAmNow_Defects>("defects");

var web = builder.AddProject<Projects.CastAmNow_Web>("web")
    .WithReference(defects);

builder.Build().Run();
