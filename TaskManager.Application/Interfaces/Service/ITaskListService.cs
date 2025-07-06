using TaskManager.Application.DTO.TaskList;

namespace TaskManager.Application.Interfaces.Service
{
    /// <summary>
    /// Інтерфейс сервісу для роботи зі списками задач
    /// </summary>
    public interface ITaskListService
    {
        Task<int> CreateAsync(PostTaskListDto dto, int userId);

        Task<IEnumerable<GetTaskListForListDto>> GetAllAsync(int userId, int page, int pageSize);

        Task<GetTaskListDto?> GetByIdAsync(int id, int userId);

        Task<bool> UpdateAsync(int id, PutTaskListDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);
    }
}