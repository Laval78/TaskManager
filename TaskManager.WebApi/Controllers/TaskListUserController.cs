using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTO.TaskListUser;
using TaskManager.Application.Interfaces.Service;

namespace TaskManager.WebApi.Controllers;

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
    /// Додати користувача до списку задач
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddUserToTaskList(int taskListId, [FromBody] PostTaskListUserDto dto)
    {
        var success = await _taskListUserService.AddUserToTaskListAsync(taskListId, dto, UserId);
        if (!success)
            return Forbid("Неможливо додати користувача до списку задач");

        return Ok(new { message = "Користувача додано до списку задач" });
    }

    /// <summary>
    /// Отримати всіх користувачів, які мають доступ до списку задач
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUsersForTaskList(int taskListId)
    {
        var result = await _taskListUserService.GetUsersForTaskListAsync(taskListId, UserId);
        return Ok(result);
    }

    /// <summary>
    /// Видалити користувача зі списку задач
    /// </summary>
    [HttpDelete("{userIdToRemove}")]
    public async Task<IActionResult> RemoveUserFromTaskList(int taskListId, int userIdToRemove)
    {
        var success = await _taskListUserService.RemoveUserFromTaskListAsync(taskListId, userIdToRemove, UserId);
        if (!success)
            return Forbid("Ви не маєте прав для видалення цього користувача");

        return Ok(new { message = "Користувача видалено зі списку задач" });
    }
}