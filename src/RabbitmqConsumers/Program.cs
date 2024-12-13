using RabbitmqConsumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddInfrastructureServices();
var host = builder.Build();
host.Run();