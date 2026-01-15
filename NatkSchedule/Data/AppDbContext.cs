using Microsoft.EntityFrameworkCore;
using NatkSchedule.Models;
using Npgsql;

namespace NatkSchedule.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Building> Buildings { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<Weekday> Weekdays { get; set; }
    public DbSet<LessonTime> LessonTimes { get; set; }
    public DbSet<Schedule> Schedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<LessonGroupPart>();

        modelBuilder.Entity<Schedule>()
            .Property(s => s.GroupPart)
            .HasConversion<string>();

        modelBuilder.Entity<Building>()
            .HasIndex(b => b.Address)
            .IsUnique();

        modelBuilder.Entity<Classroom>()
            .HasIndex(c => new { c.BuildingId, c.RoomNumber })
            .IsUnique();

        modelBuilder.Entity<Subject>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<StudentGroup>()
            .HasIndex(g => g.GroupName)
            .IsUnique();

        modelBuilder.Entity<Weekday>()
            .HasIndex(w => w.Name)
            .IsUnique();

        modelBuilder.Entity<LessonTime>()
            .HasIndex(lt => new { lt.LessonNumber, lt.TimeStart, lt.TimeEnd })
            .IsUnique();

        modelBuilder.Entity<Schedule>()
            .HasIndex(s => new { s.LessonDate, s.LessonTimeId, s.GroupId, s.GroupPart })
            .IsUnique();

        modelBuilder.Entity<Schedule>()
            .HasIndex(s => new { s.LessonDate, s.LessonTimeId, s.ClassroomId })
            .IsUnique();
    }
}
