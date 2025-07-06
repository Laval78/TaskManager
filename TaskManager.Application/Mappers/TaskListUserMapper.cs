using TaskManager.Application.DTO.TaskListUser;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Mappers
{
    public static class TaskListUserMapper
    {
        /// <summary>
        /// Мапінг моделі TaskListUser до DTO (отримання користувачів, які мають доступ до списку)
        /// </summary>
        public static GetTaskListUserDto ToGetTaskListUserDto(TaskListUser taskListUser)
        {
            return new GetTaskListUserDto
            {
                IdUser = taskListUser.IdUser,
                UserName = taskListUser.IdUserNavigation?.Name ?? "Невідомо"
            };
        }

        /// <summary>
        /// Мапінг Post DTO до моделі TaskListUser (створення зв’язку)
        /// </summary>
        public static TaskListUser ToTaskListUser(int taskListId, PostTaskListUserDto dto)
        {
            return new TaskListUser
            {
                IdTask = taskListId,
                IdUser = dto.UserId
            };
        }
    }
}
