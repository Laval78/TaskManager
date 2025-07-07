using System;
using TaskManager.Application.DTO.TaskListUser;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces.Repositories;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Services;

public class TaskListUserService : ITaskListUserService
{
    private readonly ITaskListUserRepository _taskListUserRepository;
    private readonly ILoggerManager _logger;

    public TaskListUserService(ITaskListUserRepository taskListUserRepository, ILoggerManager logger)
    {
        _taskListUserRepository = taskListUserRepository;
        _logger = logger;
    }

    public async Task<bool> AddUserToTaskListAsync(int taskListId, PostTaskListUserDto dto, int currentUserId)
    {
        _logger.LogInfo($"Спроба додати користувача {dto.UserId} до списку задач {taskListId} користувачем {currentUserId}");

        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);
        if (taskList == null)
        {
            _logger.LogWarn($"Список задач {taskListId} не знайдено");
            throw new NotFoundException("Список задач не знайдено");
        }

        if (dto.UserId == currentUserId)
        {
            _logger.LogWarn($"Користувач {currentUserId} спробував додати самого себе до списку {taskListId}");
            throw new BadRequestException("Неможливо додати самого себе до списку задач");
        }

        if (taskList.WhoCreated != currentUserId)
        {
            _logger.LogWarn($"Користувач {currentUserId} не є власником списку {taskListId}");
            throw new ForbiddenException("Тільки власник може додати користувача до списку задач");
        }

        if (taskList.TaskListUsers.Any(x => x.IdUser == dto.UserId))
        {
            _logger.LogWarn($"Користувач {dto.UserId} вже доданий до списку {taskListId}");
            throw new BadRequestException("Цей користувач вже доданий до списку задач");
        }

        if (taskList.TaskListUsers.Count >= 3)
        {
            _logger.LogWarn($"Список {taskListId} вже має 3 користувачів");
            throw new BadRequestException("До списку задач можна додати максимум 3-х користувачів");
        }

        var newLink = new TaskListUser
        {
            IdTask = taskListId,
            IdUser = dto.UserId
        };

        await _taskListUserRepository.AddUserToTaskListAsync(newLink);

        _logger.LogInfo($"Користувача {dto.UserId} успішно додано до списку {taskListId}");
        return true;
    }

    public async Task<IEnumerable<GetTaskListUserDto>> GetUsersForTaskListAsync(int taskListId, int currentUserId)
    {
        _logger.LogInfo($"Отримання користувачів для списку {taskListId} запрошено користувачем {currentUserId}");

        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);

        if (taskList == null)
        {
            _logger.LogWarn($"Список задач {taskListId} не знайдено");
            throw new NotFoundException("Список задач не знайдено");
        }

        if (taskList.WhoCreated != currentUserId &&
            !taskList.TaskListUsers.Any(x => x.IdUser == currentUserId))
        {
            _logger.LogWarn($"Користувач {currentUserId} не має доступу до списку {taskListId}");
            throw new ForbiddenException("У вас немає доступу до цього списку задач");
        }

        _logger.LogInfo($"Успішно отримано список користувачів для списку {taskListId}");

        return taskList.TaskListUsers.Select(tu => new GetTaskListUserDto
        {
            IdUser = tu.IdUser,
            UserName = tu.IdUserNavigation?.Name ?? "Невідомо"
        });
    }

    public async Task<bool> RemoveUserFromTaskListAsync(int taskListId, int userIdToRemove, int currentUserId)
    {
        _logger.LogInfo($"Спроба видалити користувача {userIdToRemove} зі списку {taskListId} користувачем {currentUserId}");

        var taskList = await _taskListUserRepository.GetTaskListWithUsersAsync(taskListId);

        if (taskList == null)
        {
            _logger.LogWarn($"Список задач {taskListId} не знайдено");
            throw new NotFoundException("Список задач не знайдено");
        }

        if (taskList.WhoCreated != currentUserId && currentUserId != userIdToRemove)
        {
            _logger.LogWarn($"Користувач {currentUserId} не має прав на видалення користувача {userIdToRemove} зі списку {taskListId}");
            throw new ForbiddenException("Тільки власник або сам користувач може видалити доступ");
        }

        var link = taskList.TaskListUsers.FirstOrDefault(tu => tu.IdUser == userIdToRemove);
        if (link == null)
        {
            _logger.LogWarn($"Користувача {userIdToRemove} не знайдено у списку {taskListId}");
            throw new NotFoundException("Цей користувач не знайдений у списку задач");
        }

        await _taskListUserRepository.RemoveUserFromTaskListAsync(link);

        _logger.LogInfo($"Користувача {userIdToRemove} успішно видалено зі списку {taskListId}");
        return true;
    }
}