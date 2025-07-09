namespace ProductService.Events;

public class ProductEvents
{
    public static event Action<string> OnLog;

    public static void LogMessage(string msg)
    {
        OnLog?.Invoke(msg);
    }
}