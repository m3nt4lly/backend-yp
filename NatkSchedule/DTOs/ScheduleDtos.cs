using NatkSchedule.Models;

namespace NatkSchedule.DTOs;

public class LessonPartDto
{
    public required string SubjectName { get; set; }
    public required string TeacherName { get; set; }
    public required string ClassroomNumber { get; set; }
    public required string BuildingName { get; set; }
}

public class LessonDto
{
    public int LessonNumber { get; set; }
    public string TimeStart { get; set; } = string.Empty;
    public string TimeEnd { get; set; } = string.Empty;
    
    public Dictionary<string, LessonPartDto> Parts { get; set; } = new();
}

public class ScheduleByDateDto
{
    public DateOnly Date { get; set; }
    public required string WeekdayName { get; set; }
    public List<LessonDto> Lessons { get; set; } = new();
}
