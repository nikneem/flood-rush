var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.HexMaster_FloodRush_App>("hexmaster-floodrush-app");

builder.Build().Run();
