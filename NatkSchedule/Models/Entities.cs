using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatkSchedule.Models;

[Table("building")]
public class Building
{
    [Key]
    [Column("building_id")]
    public int BuildingId { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    [Column("address")]
    public required string Address { get; set; }
}

[Table("classroom")]
public class Classroom
{
    [Key]
    [Column("classroom_id")]
    public int ClassroomId { get; set; }

    [Column("building_id")]
    public int BuildingId { get; set; }
    [ForeignKey("BuildingId")]
    public Building? Building { get; set; }

    [Column("room_number")]
    public required string RoomNumber { get; set; }
}

[Table("teacher")]
public class Teacher
{
    [Key]
    [Column("teacher_id")]
    public int TeacherId { get; set; }

    [Column("last_name")]
    public required string LastName { get; set; }

    [Column("first_name")]
    public required string FirstName { get; set; }

    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Column("position")]
    public required string Position { get; set; }
    
    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
}

[Table("subject")]
public class Subject
{
    [Key]
    [Column("subject_id")]
    public int SubjectId { get; set; }

    [Column("name")]
    public required string Name { get; set; }
}

[Table("specialties")]
public class Specialty
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }
}

[Table("student_group")]
public class StudentGroup
{
    [Key]
    [Column("group_id")]
    public int GroupId { get; set; }

    [Column("group_name")]
    public required string GroupName { get; set; }

    [Column("course")]
    public int Course { get; set; }

    [Column("specialty_id")]
    public int SpecialtyId { get; set; }
    [ForeignKey("SpecialtyId")]
    public Specialty? Specialty { get; set; }
}

[Table("weekday")]
public class Weekday
{
    [Key]
    [Column("weekday_id")]
    public int WeekdayId { get; set; }

    [Column("name")]
    public required string Name { get; set; }
}

[Table("lesson_time")]
public class LessonTime
{
    [Key]
    [Column("lesson_time_id")]
    public int LessonTimeId { get; set; }

    [Column("lesson_number")]
    public int LessonNumber { get; set; }

    [Column("time_start")]
    public TimeOnly TimeStart { get; set; }

    [Column("time_end")]
    public TimeOnly TimeEnd { get; set; }
}

public enum LessonGroupPart
{
    FULL,
    SUB1,
    SUB2
}

[Table("schedule")]
public class Schedule
{
    [Key]
    [Column("schedule_id")]
    public int ScheduleId { get; set; }

    [Column("lesson_date")]
    public DateOnly LessonDate { get; set; }

    [Column("weekday_id")]
    public int WeekdayId { get; set; }
    [ForeignKey("WeekdayId")]
    public Weekday? Weekday { get; set; }

    [Column("lesson_time_id")]
    public int LessonTimeId { get; set; }
    [ForeignKey("LessonTimeId")]
    public LessonTime? LessonTime { get; set; }

    [Column("group_id")]
    public int GroupId { get; set; }
    [ForeignKey("GroupId")]
    public StudentGroup? StudentGroup { get; set; }

    [Column("group_part")]
    public LessonGroupPart GroupPart { get; set; }

    [Column("subject_id")]
    public int SubjectId { get; set; }
    [ForeignKey("SubjectId")]
    public Subject? Subject { get; set; }

    [Column("teacher_id")]
    public int TeacherId { get; set; }
    [ForeignKey("TeacherId")]
    public Teacher? Teacher { get; set; }

    [Column("classroom_id")]
    public int ClassroomId { get; set; }
    [ForeignKey("ClassroomId")]
    public Classroom? Classroom { get; set; }
}
