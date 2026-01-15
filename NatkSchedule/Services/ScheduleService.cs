using Microsoft.EntityFrameworkCore;
using NatkSchedule.Data;
using NatkSchedule.DTOs;
using NatkSchedule.Models;

namespace NatkSchedule.Services;

public class ScheduleService : IScheduleService
{
    private readonly AppDbContext _context;

    public ScheduleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetGroupsAsync()
    {
        return await _context.StudentGroups
            .OrderBy(g => g.GroupName)
            .Select(g => g.GroupName)
            .ToListAsync();
    }

    public async Task<List<ScheduleByDateDto>> GetScheduleAsync(string groupName, DateTime start, DateTime end)
    {
        var startDate = DateOnly.FromDateTime(start);
        var endDate = DateOnly.FromDateTime(end);

        var group = await _context.StudentGroups.FirstOrDefaultAsync(g => g.GroupName == groupName);
        if (group == null)
        {
            throw new Exception($"Group {groupName} not found");
        }

        var schedules = await _context.Schedules
            .Include(s => s.LessonTime)
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .Include(s => s.Classroom)
                .ThenInclude(c => c.Building)
            .Include(s => s.Weekday)
            .Where(s => s.GroupId == group.GroupId && s.LessonDate >= startDate && s.LessonDate <= endDate)
            .ToListAsync();

        var lessonTimes = await _context.LessonTimes.OrderBy(lt => lt.LessonNumber).ToListAsync();

        var result = new List<ScheduleByDateDto>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var dayOfWeekRu = date.ToString("dddd", new System.Globalization.CultureInfo("ru-RU"));
            
            if (!string.IsNullOrEmpty(dayOfWeekRu))
            {
                dayOfWeekRu = char.ToUpper(dayOfWeekRu[0]) + dayOfWeekRu.Substring(1);
            }

            var dailySchedule = new ScheduleByDateDto
            {
                Date = date,
                WeekdayName = dayOfWeekRu
            };

            foreach (var lt in lessonTimes)
            {
                var lessonDto = new LessonDto
                {
                    LessonNumber = lt.LessonNumber,
                    TimeStart = lt.TimeStart.ToString("HH:mm"),
                    TimeEnd = lt.TimeEnd.ToString("HH:mm")
                };

                var lessons = schedules
                    .Where(s => s.LessonDate == date && s.LessonTimeId == lt.LessonTimeId)
                    .ToList();

                foreach (var lesson in lessons)
                {
                    if (lesson.Subject == null || lesson.Teacher == null || lesson.Classroom == null) continue;

                    var partDto = new LessonPartDto
                    {
                        SubjectName = lesson.Subject.Name,
                        TeacherName = lesson.Teacher.FullName,
                        ClassroomNumber = lesson.Classroom.RoomNumber,
                        BuildingName = lesson.Classroom.Building?.Name ?? "?"
                    };

                    lessonDto.Parts[lesson.GroupPart.ToString()] = partDto;
                }

                dailySchedule.Lessons.Add(lessonDto);
            }

            result.Add(dailySchedule);
        }

        return result;
    }
}
