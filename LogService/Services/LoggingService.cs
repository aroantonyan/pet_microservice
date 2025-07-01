using LogService.Interfaces;
using Serilog;

namespace LogService.Services;

public class LoggingService : ILogInterface
{
    public bool LogMessage(string message)
    {
        Log.Information(message);   
        return true;
    }
}