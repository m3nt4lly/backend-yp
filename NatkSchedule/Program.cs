using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using NatkSchedule.Data;
using NatkSchedule.Middleware;
using NatkSchedule.Services;

var builder = WebApplication.CreateBuilder(args);

// Загрузка .env файла
Env.Load();

// Получение строки подключения
// Сначала пробуем из переменных окружения (Docker/EnvFile), затем из appsettings
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IScheduleService, ScheduleService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Включаем Swagger всегда для удобства проверки (в учебном проекте)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NatkSchedule API V1");
    c.RoutePrefix = "swagger"; 
});

// Редирект с корня на Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// app.UseHttpsRedirection(); // Отключаем для упрощения локальной разработки с эмулятором

app.UseAuthorization();

app.MapControllers();

app.Run();
