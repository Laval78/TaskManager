using TaskManager.Domain.Models;

namespace TaskManager.Application.Interfaces.Repositories;

public interface ITaskListUserRepository
{
    Task<TaskList?> GetTaskListWithUsersAsync(int taskListId);

    Task AddUserToTaskListAsync(TaskListUser taskListUser);

    Task RemoveUserFromTaskListAsync(TaskListUser taskListUser);

    Task SaveChangesAsync();
}