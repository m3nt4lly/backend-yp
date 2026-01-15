using Microsoft.AspNetCore.Mvc;
using NatkSchedule.Services;

namespace NatkSchedule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await _scheduleService.GetGroupsAsync();
        return Ok(groups);
    }

    [HttpGet("group/{groupName}")]
    public async Task<IActionResult> GetSchedule(string groupName, [FromQuery] string start, [FromQuery] string end)
    {
        if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
        {
            var now = DateTime.Now;
            var startDate = now;
            var endDate = now.AddDays(7);
            var schedule = await _scheduleService.GetScheduleAsync(groupName, startDate, endDate);
            return Ok(schedule);
        }

        try
        {
            var startDate = DateTime.Parse(start);
            var endDate = DateTime.Parse(end);
            var schedule = await _scheduleService.GetScheduleAsync(groupName, startDate, endDate);
            return Ok(schedule);
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(ex.Message);
        }
    }
}
