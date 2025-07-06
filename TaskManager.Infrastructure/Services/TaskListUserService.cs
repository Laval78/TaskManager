using TaskManager.Application.DTO.TaskListUser;
using TaskManager.Application.Interfaces.Repositories;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Services;

public class TaskListUserService : ITaskListUserService
{
    private readonly ITaskListUserRepository _taskListUserRepository;

    public TaskListUserService(ITaskListUserRepository taskListUserRepository)
    {
        _taskListUserRepository = taskListUserRepository;
    }

    public async Task<bool> AddUserToTaskListAsync(int taskListId, PostTaskListUserDto dto, int currentUserId)
    {
        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);

        if (taskList == null || taskList.WhoCreated != currentUserId)
            return false;

        if (taskList.TaskListUsers.Count >= 3)
            return false;

        if (taskList.TaskListUsers.Any(x => x.IdUser == dto.UserId))
            return false;

        var newLink = new TaskListUser
        {
            IdTask = taskListId,
            IdUser = dto.UserId
        };

        await _taskListUserRepository.AddUserToTaskListAsync(newLink);
        return true;
    }

    public async Task<IEnumerable<GetTaskListUserDto>> GetUsersForTaskListAsync(int taskListId, int currentUserId)
    {
        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);

        if (taskList == null || (taskList.WhoCreated != currentUserId &&
            !taskList.TaskListUsers.Any(x => x.IdUser == currentUserId)))
            return Enumerable.Empty<GetTaskListUserDto>();

        return taskList.TaskListUsers.Select(tu => new GetTaskListUserDto
        {
            IdUser = tu.IdUser,
            UserName = tu.IdUserNavigation?.Name ?? "Невідомо"
        });
    }

    public async Task<bool> RemoveUserFromTaskListAsync(int taskListId, int userIdToRemove, int currentUserId)
    {
        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);

        if (taskList == null || (taskList.WhoCreated != currentUserId && currentUserId != userIdToRemove))
            return false;

        var link = taskList.TaskListUsers.FirstOrDefault(tu => tu.IdUser == userIdToRemove);
        if (link == null)
            return false;

        await _taskListUserRepository.RemoveUserFromTaskListAsync(link);
        return true;
    }
}