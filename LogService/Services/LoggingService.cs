using LogService.Dto;
using LogService.Interfaces;
using Serilog;

namespace LogService.Services;

public class LoggingService : ILogInterface
{
    public Task LogAsync(LogDto log)
    {
        Log.Logger.Information(
            "Log received: Message='{Message}', Level='{Level}', Source='{Source}', Path='{Path}', TraceId='{TraceId}', Exception='{Exception}'",
            log.Message,
            log.Level,
            log.Source,
            log.Path,
            log.TraceId,
            log.Exception
        );

        return Task.CompletedTask;
    }
}