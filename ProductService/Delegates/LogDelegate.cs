namespace ProductService.Delegates;

public static class LogDelegate
{
    public static Action<string> LogToConsole = msg => Console.WriteLine(msg);
}