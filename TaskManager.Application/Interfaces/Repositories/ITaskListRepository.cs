using TaskManager.Domain.Models;

public interface ITaskListRepository
{
    Task<int> CreateAsync(TaskList taskList);

    Task<IEnumerable<TaskList>> GetAllForUserAsync(int userId, int page, int pageSize);

    Task<TaskList?> GetByIdWithCreatorAsync(int id, int userId);

    Task<TaskList?> GetByIdAsync(int id);

    Task<bool> DeleteAsync(TaskList taskList);

    Task SaveChangesAsync();
}