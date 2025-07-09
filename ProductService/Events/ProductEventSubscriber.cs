namespace ProductService.Events;

public class ProductEventSubscriber : IDisposable
{
    private bool _disposed;
    public ProductEventSubscriber()
    {
        ProductEvents.OnLog += LogToConsole1;
        ProductEvents.OnLog += LogToConsole2;
    }

    private void LogToConsole1(string msg)
    {
        Console.WriteLine("LogToConsole1 -" + msg);
    }

    private void LogToConsole2(string msg)
    {
        Console.WriteLine("LogToConsole2 -" + msg);
    }

    public void Dispose()
    {
        if (_disposed) return;
        ProductEvents.OnLog -= LogToConsole1;
        ProductEvents.OnLog -= LogToConsole2;
        _disposed = true;
    }
}