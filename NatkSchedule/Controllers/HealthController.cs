using Microsoft.AspNetCore.Mvc;

namespace NatkSchedule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "Healthy", time = DateTime.Now });
    }
}
