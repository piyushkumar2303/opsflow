using Microsoft.AspNetCore.Mvc;

namespace OpsFlow.Api.Controllers;

[ApiController]
[Route("api/system")]
public class SystemController : ControllerBase
{
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            Service = "OpsFlow.Api",
            Status = "Running"
        });
    }
}