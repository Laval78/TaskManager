using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces.Repositories;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Configuration
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json", optional: false)
.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
.AddEnvironmentVariables();

// Підключення контексту БД
builder.Services.AddDbContext<TaskManagerContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
b => b.MigrationsAssembly("TaskManager.Infrastructure")));

// Реєстрація сервісів у DI контейнері
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskListService, TaskListService>();
builder.Services.AddScoped<ITaskListUserService, TaskListUserService>();
builder.Services.AddScoped<ITaskListUserRepository, TaskListUserRepository>();
builder.Services.AddScoped<ITaskListUserService, TaskListUserService>();

// Swagger + MVC
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TaskManager.WebApi", Version = "v1" });

    // Добавляем заголовок X-User-Id как глобальный параметр
    c.OperationFilter<AddRequiredHeaderParameter>();
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<RequireUserIdHeaderMiddleware>();

app.MapControllers();

app.Run();