using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;
using TaskManager.Application.Interfaces.Repositories;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Infrastructure.Seed;
using TaskManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();
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
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskListService, TaskListService>();
builder.Services.AddScoped<ITaskListUserService, TaskListUserService>();
builder.Services.AddScoped<ITaskListUserRepository, TaskListUserRepository>();
builder.Services.AddScoped<ITaskListUserService, TaskListUserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TaskManager.WebApi", Version = "v1" });

    // Додає заголовок User-Id як глобальний параметр
    c.OperationFilter<AddRequiredHeaderParameter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// true якщо треба оновити дані в таблицях БД
//if (true)
//{
//    using var scope = app.Services.CreateScope();
//    var context = scope.ServiceProvider.GetRequiredService<TaskManagerContext>();
//    using var transaction = context.Database.BeginTransaction();

//    SeedDatabase.Clear(context);
//    await SeedDatabase.Seed(context);

//    transaction.Commit();
//}

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