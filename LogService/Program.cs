using Hangfire;
using Hangfire.PostgreSql;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();                      
builder.Logging.AddFilter("Hangfire", LogLevel.Warning); 

builder.Logging.AddSerilog();

var cs = builder.Configuration.GetConnectionString("HangfireConnection");

builder.Services.AddHangfire(cfg => cfg.UsePostgreSqlStorage(cs));
builder.Services.AddHangfireServer();

builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();
app.Run();