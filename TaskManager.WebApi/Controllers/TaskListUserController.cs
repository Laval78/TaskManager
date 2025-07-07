using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTO.TaskListUser;
using TaskManager.Application.Interfaces.Service;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/tasklists/{taskListId}/users")]
    [RequireUserIdHeader]
    public class TaskListUserController : ControllerBase
    {
        private readonly ITaskListUserService _taskListUserService;

        public TaskListUserController(ITaskListUserService taskListUserService)
        {
            _taskListUserService = taskListUserService;
        }

        private int UserId => HttpContext.Items.TryGetValue("UserId", out var value) && value is int id
            ? id
            : throw new UnauthorizedAccessException("User ID is missing from request context.");

        /// <summary>
        /// Додає користувача до вказаного списку задач
        /// </summary>
        /// <param name="taskListId">ID списку задач, до якого додається користувач</param>
        /// <param name="dto">Обʼєкт з даними користувача, якого потрібно додати</param>
        /// <returns>Підтвердження успішного додавання</returns>
        [HttpPost]
        public async Task<IActionResult> AddUserToTaskList(int taskListId, [FromBody] PostTaskListUserDto dto)
        {
            await _taskListUserService.AddUserToTaskListAsync(taskListId, dto, UserId);
            return Ok(new { message = "Користувача додано до списку задач" });
        }

        /// <summary>
        /// Отримує список користувачів, які мають доступ до певного списку задач
        /// </summary>
        /// <param name="taskListId">ID списку задач</param>
        /// <returns>Список користувачів, повʼязаних зі списком задач</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsersForTaskList(int taskListId)
        {
            var result = await _taskListUserService.GetUsersForTaskListAsync(taskListId, UserId);
            return Ok(result);
        }

        /// <summary>
        /// Видаляє користувача зі списку задач
        /// </summary>
        /// <param name="taskListId">ID списку задач</param>
        /// <param name="userIdToRemove">ID користувача, якого потрібно видалити</param>
        /// <returns>Підтвердження успішного видалення</returns>
        [HttpDelete("{userIdToRemove}")]
        public async Task<IActionResult> RemoveUserFromTaskList(int taskListId, int userIdToRemove)
        {
            await _taskListUserService.RemoveUserFromTaskListAsync(taskListId, userIdToRemove, UserId);
            return Ok(new { message = "Користувача видалено зі списку задач" });
        }
    }
}

