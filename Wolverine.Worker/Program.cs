var builder = Host.CreateApplicationBuilder(args);

builder.AddWorker();

var host = builder.Build();
host.Run();

