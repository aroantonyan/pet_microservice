using Hangfire;
using Hangfire.PostgreSql;
using LogService.Interfaces;
using LogService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddSerilog();

var cs = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(cfg => cfg.UsePostgreSqlStorage(cs));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ILogInterface, LoggingService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard(); 

app.Run();