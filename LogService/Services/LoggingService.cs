using LogService.Dto;
using LogService.Interfaces;
using Serilog;

namespace LogService.Services;

public class LoggingService : ILogInterface
{
    public Task LogAsync(LogDto log)
    {
        Log.Logger.ForContext("Source", log.Source)
            .ForContext("Path", log.Path)
            .ForContext("TraceId", log.TraceId)
            .ForContext("Level", log.Level)
            .ForContext("Exception", log.Exception)
            .Information("{Message}", log.Message);

        return Task.CompletedTask;
    }
}