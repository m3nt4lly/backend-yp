using Microsoft.EntityFrameworkCore;
using NatkSchedule.Models;
using Npgsql;

namespace NatkSchedule.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // Включаем поддержку TimeOnly/DateOnly и Enums
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Обычно строка подключения в Program.cs, но здесь можно добавить логирование
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Регистрация Enum
        modelBuilder.HasPostgresEnum<LessonGroupPart>();

        modelBuilder.Entity<Schedule>()
            .Property(s => s.GroupPart)
            .HasConversion<string>(); // Или используем нативный Enum, если NpgsqlDataSourceBuilder настроен

        // Уникальные индексы (из SQL)
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
