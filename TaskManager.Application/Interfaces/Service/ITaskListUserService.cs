using TaskManager.Application.DTO.TaskListUser;

namespace TaskManager.Application.Interfaces.Service
{
    /// <summary>
    /// Інтерфейс сервіс для керування користувачами в списках задач.
    /// </summary>
    public interface ITaskListUserService
    {
        Task<bool> AddUserToTaskListAsync(int taskListId, PostTaskListUserDto dto, int currentUserId);

        Task<IEnumerable<GetTaskListUserDto>> GetUsersForTaskListAsync(int taskListId, int currentUserId);

        Task<bool> RemoveUserFromTaskListAsync(int taskListId, int userIdToRemove, int currentUserId);
    }
}