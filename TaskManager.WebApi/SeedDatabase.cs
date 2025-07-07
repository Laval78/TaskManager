using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Seed;

public static class SeedDatabase
{
    public static void Clear(TaskManagerContext context)
    {
        context.Database.ExecuteSqlRaw(@"DELETE FROM ""TM"".""TaskListUsers""");
        context.Database.ExecuteSqlRaw(@"DELETE FROM ""TM"".""TaskList""");
        context.Database.ExecuteSqlRaw(@"DELETE FROM ""TM"".""Users""");
        context.SaveChanges();
    }

    public static async Task Seed(TaskManagerContext context)
    {
        #region
        var users = new List<User>
        {
            new(1, "Іван"),
            new(2, "Марія"),
            new(3, "Олег"),
            new(4, "Олена"),
            new(5, "Сергій"),
            new(6, "Наталія"),
            new(7, "Андрій")
        };
        #endregion

        context.Users.AddRange(users);
        context.SaveChanges();


        #region TaskList
        var taskLists = new List<TaskList>
        {
        new(1, "Задачі Івана", new DateTime(2024, 12, 15, 10, 0, 0, DateTimeKind.Utc), 1),
        new(2, "Новий проєкт", new DateTime(2024, 12, 18, 9, 30, 0, DateTimeKind.Utc), 2),
        new(3, "Особисте", new DateTime(2024, 12, 20, 15, 45, 0, DateTimeKind.Utc), 3),
        new(4, "Відпустка", new DateTime(2025, 1, 5, 12, 0, 0, DateTimeKind.Utc), 4),
        new(5, "Покупки", new DateTime(2025, 1, 7, 18, 20, 0, DateTimeKind.Utc), 5),
        new(6, "Домашні справи", new DateTime(2025, 1, 10, 8, 10, 0, DateTimeKind.Utc), 6),
        new(7, "Робочі плани", new DateTime(2025, 1, 15, 14, 0, 0, DateTimeKind.Utc), 7),
        new(8, "Навчання", new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc), 1),
        new(9, "Зустрічі", new DateTime(2025, 2, 3, 11, 0, 0, DateTimeKind.Utc), 2),
        new(10, "Фінанси", new DateTime(2025, 2, 5, 13, 0, 0, DateTimeKind.Utc), 3),
        new(12, "Планування року", new DateTime(2025, 2, 7, 17, 0, 0, DateTimeKind.Utc), 5),
        new(11, "Ідеї", new DateTime(2025, 2, 6, 16, 0, 0, DateTimeKind.Utc), 4)
        };
        #endregion

        context.TaskLists.AddRange(taskLists);
        context.SaveChanges();

        #region TaskListUser
        var taskListUsers = new List<TaskListUser>
        {
            new(1, 1, 2),
            new(2, 1, 3),
            new(3, 2, 1),
            new(4, 3, 4),
            new(5, 4, 5),
            new(6, 5, 6),
            new(7, 6, 7),
            new(8, 7, 1),
            new(9, 8, 2),
            new(10, 9, 3),
            new(11, 10, 4),
            new(12, 11, 5),
            new(13, 12, 6)
        };
        #endregion

        context.TaskListUsers.AddRange(taskListUsers);
        await context.SaveChangesAsync();
    }
}