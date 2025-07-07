using TaskManager.Application.DTO.TaskList;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Application.Mappers;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Services;

/// <summary>
/// Сервіс для роботи зі списками задач
/// </summary>
public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _repository;
    private readonly ILoggerManager _logger;

    public TaskListService(ITaskListRepository repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> CreateAsync(PostTaskListDto dto, int userId)
    {
        _logger.LogInfo($"Створення списку задач користувачем {userId}");

        var taskList = new TaskList
        {
            Title = dto.Title,
            DateTimeCreated = DateTime.UtcNow,
            WhoCreated = userId
        };

        var id = await _repository.CreateAsync(taskList);

        _logger.LogInfo($"Список задач з Id {id} створено успішно");

        return id;
    }

    public async Task<IEnumerable<GetTaskListForListDto>> GetAllAsync(int userId, int page, int pageSize)
    {
        _logger.LogInfo($"Отримання списку задач користувача {userId}, сторінка {page}, розмір {pageSize}");

        var taskLists = await _repository.GetAllForUserAsync(userId, page, pageSize);
        return taskLists.Select(t => t.ToGetTaskListForListDto());
    }

    public async Task<GetTaskListDto> GetByIdAsync(int id, int userId)
    {
        _logger.LogInfo($"Запит на отримання списку задач Id {id} від користувача {userId}");

        var taskList = await _repository.GetByIdWithCreatorAsync(id, userId);
        if (taskList == null)
        {
            _logger.LogWarn($"Список задач Id {id} не знайдено або доступ заборонено для користувача {userId}");
            throw new NotFoundException("Список задач не знайдено або у вас немає до нього доступу");
        }

        return taskList.ToGetTaskListDto();
    }

    public async Task UpdateAsync(int id, PutTaskListDto dto, int userId)
    {
        _logger.LogInfo($"Оновлення списку задач Id {id} користувачем {userId}");

        var taskList = await _repository.GetByIdAsync(id);

        if (taskList == null)
        {
            _logger.LogWarn($"Список задач Id {id} не знайдено для оновлення");
            throw new NotFoundException("Список задач не знайдено");
        }

        if (taskList.WhoCreated != userId)
        {
            _logger.LogWarn($"Користувач {userId} не є власником списку задач Id {id}");
            throw new ForbiddenException("Тільки власник може змінювати список задач");
        }

        taskList.Title = dto.Title;
        await _repository.SaveChangesAsync();

        _logger.LogInfo($"Список задач Id {id} оновлено успішно");
    }

    public async Task DeleteAsync(int id, int userId)
    {
        _logger.LogInfo($"Видалення списку задач Id {id} користувачем {userId}");

        var taskList = await _repository.GetByIdAsync(id);

        if (taskList == null)
        {
            _logger.LogWarn($"Список задач Id {id} не знайдено для видалення");
            throw new NotFoundException("Список задач не знайдено");
        }

        if (taskList.WhoCreated != userId)
        {
            _logger.LogWarn($"Користувач {userId} не має прав на видалення списку Id {id}");
            throw new ForbiddenException("Тільки власник може видалити список задач");
        }

        await _repository.DeleteAsync(taskList);

        _logger.LogInfo($"Список задач Id {id} видалено успішно");
    }
}