var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CleanSharpArchitecture>("cleansharparchitecture");

builder.Build().Run();
