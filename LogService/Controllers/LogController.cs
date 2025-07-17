using Hangfire;
using LogService.Dto;
using LogService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LogService.Controllers;
[ApiController]
[Route("log")]
public class LogController() : ControllerBase
{
    [HttpPost("ReceiveLog")]
    public IActionResult ReceiveLog([FromBody] LogDto log)
    {
        BackgroundJob.Enqueue<ILogInterface>(logger => logger.LogAsync(log) );
        return Ok();
    }
}