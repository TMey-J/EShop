using Serilog.Events;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(path: Path.Combine(Directory.GetCurrentDirectory(), "logs", "log-.log"),
        rollingInterval: RollingInterval.Day,
        outputTemplate:
        "[{Level:u3}] | {Timestamp:yyyy-MM-dd HH:mm:ss zzz} | {SourceContext} | {Message:lj}{NewLine}{Exception}"
    ).WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.ForContext<Program>().Information("Starting application");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices(builder.Configuration);


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.ForContext<Program>().Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
