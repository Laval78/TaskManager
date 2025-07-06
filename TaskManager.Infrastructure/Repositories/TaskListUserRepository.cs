using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces.Repositories;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Repositories;

public class TaskListUserRepository : ITaskListUserRepository
{
    private readonly TaskManagerContext _context;

    public TaskListUserRepository(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task<TaskList?> GetTaskListWithUsersAsync(int taskListId)
    {
        return await _context.TaskLists
            .Include(t => t.TaskListUsers)
                .ThenInclude(tu => tu.IdUserNavigation)
            .FirstOrDefaultAsync(t => t.Id == taskListId);
    }

    public async Task AddUserToTaskListAsync(TaskListUser taskListUser)
    {
        _context.TaskListUsers.Add(taskListUser);
        await SaveChangesAsync();
    }

    public async Task RemoveUserFromTaskListAsync(TaskListUser taskListUser)
    {
        _context.TaskListUsers.Remove(taskListUser);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}