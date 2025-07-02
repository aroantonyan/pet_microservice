using System.Text;
using System.Text.Json;

namespace ProductService.Services;

public class LogSenderService(HttpClient client)
{
    private const string Endpoint = "/log/ReceiveLog";   

    public async Task SendLogAsync(string message)
    {
        var payload  = JsonSerializer.Serialize(new { Message = message });
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var resp = await client.PostAsync(Endpoint, content);
        resp.EnsureSuccessStatusCode();
    }
}