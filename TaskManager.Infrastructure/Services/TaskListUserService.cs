using TaskManager.Application.DTO.TaskListUser;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces.Repositories;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Services;

public class TaskListUserService : ITaskListUserService
{
    private readonly ITaskListUserRepository _taskListUserRepository;

    public TaskListUserService(ITaskListUserRepository taskListUserRepository, ITaskListRepository @object)
    {
        _taskListUserRepository = taskListUserRepository;
    }

    public async Task<bool> AddUserToTaskListAsync(int taskListId, PostTaskListUserDto dto, int currentUserId)
    {
        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);
        if (taskList == null)
        {
            throw new NotFoundException("Список задач не знайдено");
        }

        if (dto.UserId == currentUserId)
        {
            throw new BadRequestException("Неможливо додати самого себе до списку задач");
        }

        if (taskList.WhoCreated != currentUserId)
        {
            throw new ForbiddenException("Тільки власник може додати користувача до списку задач");
        }

        if (taskList.TaskListUsers.Any(x => x.IdUser == dto.UserId))
        {
            throw new BadRequestException("Цей користувач вже доданий до списку задач");
        }

        if (taskList.TaskListUsers.Count >= 3)
        {
            throw new BadRequestException("До списку задач можна додати максимум 3-х користувачів");
        }

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

        if (taskList == null)
            throw new NotFoundException("Список задач не знайдено");

        if (taskList.WhoCreated != currentUserId &&
            !taskList.TaskListUsers.Any(x => x.IdUser == currentUserId))
            throw new ForbiddenException("У вас немає доступу до цього списку задач");

        return taskList.TaskListUsers.Select(tu => new GetTaskListUserDto
        {
            IdUser = tu.IdUser,
            UserName = tu.IdUserNavigation?.Name ?? "Невідомо"
        });
    }

    public async Task<bool> RemoveUserFromTaskListAsync(int taskListId, int userIdToRemove, int currentUserId)
    {
        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);

        if (taskList == null)
            throw new NotFoundException("Список задач не знайдено");

        if (taskList.WhoCreated != currentUserId && currentUserId != userIdToRemove)
            throw new ForbiddenException("Тільки власник або сам користувач може видалити доступ");

        var link = taskList.TaskListUsers.FirstOrDefault(tu => tu.IdUser == userIdToRemove);
        if (link == null)
            throw new NotFoundException("Цей користувач не знайдений у списку задач");

        await _taskListUserRepository.RemoveUserFromTaskListAsync(link);
        return true;
    }
}