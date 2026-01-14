using NatkSchedule.DTOs;

namespace NatkSchedule.Services;

public interface IScheduleService
{
    Task<List<ScheduleByDateDto>> GetScheduleAsync(string groupName, DateTime start, DateTime end);
    Task<List<string>> GetGroupsAsync();
}
