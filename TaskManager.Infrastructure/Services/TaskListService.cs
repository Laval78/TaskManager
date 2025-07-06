using TaskManager.Application.DTO.TaskList;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Application.Mappers;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Services
{
    /// <summary>
    /// Сервіс для роботи зі списками задач
    /// </summary>
    public class TaskListService : ITaskListService
    {
        private readonly ITaskListRepository _repository;

        public TaskListService(ITaskListRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(PostTaskListDto dto, int userId)
        {
            var taskList = new TaskList
            {
                Title = dto.Title,
                DateTimeCreated = DateTime.UtcNow,
                WhoCreated = userId
            };

            return await _repository.CreateAsync(taskList);
        }

        public async Task<IEnumerable<GetTaskListForListDto>> GetAllAsync(int userId, int page, int pageSize)
        {
            var taskLists = await _repository.GetAllForUserAsync(userId, page, pageSize);
            return taskLists.Select(t => t.ToGetTaskListForListDto());
        }

        public async Task<GetTaskListDto?> GetByIdAsync(int id, int userId)
        {
            var taskList = await _repository.GetByIdWithCreatorAsync(id, userId);
            return taskList?.ToGetTaskListDto();
        }

        public async Task<bool> UpdateAsync(int id, PutTaskListDto dto, int userId)
        {
            var taskList = await _repository.GetByIdAsync(id);

            if (taskList == null || taskList.WhoCreated != userId)
                return false;

            taskList.Title = dto.Title;
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var taskList = await _repository.GetByIdAsync(id);

            if (taskList == null || taskList.WhoCreated != userId)
                return false;

            return await _repository.DeleteAsync(taskList);
        }
    }
}