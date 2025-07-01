using Hangfire;
using LogService.Dto;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LogService.Controllers;
[ApiController]
[Route("log")]
public class LogController() : ControllerBase
{
    [HttpPost("ReceiveLog")]
    public IActionResult ReceiveLog([FromBody] LogRequestDto log)
    {
        BackgroundJob.Enqueue(() => Log.Information(log.Message));
        return Ok(log);
    }
}