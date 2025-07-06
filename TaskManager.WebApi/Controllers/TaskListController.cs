using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTO.TaskList;
using TaskManager.Application.Interfaces.Service;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskListController : ControllerBase
{
    private readonly ITaskListService _taskListService;

    public TaskListController(ITaskListService taskListService)
    {
        _taskListService = taskListService;
    }

    private int GetCurrentUserId()
    {
        // Пример: передаём userId в заголовке X-User-Id
        if (int.TryParse(HttpContext.Request.Headers["User-Id"], out var userId))
            return userId;

        throw new UnauthorizedAccessException("User ID header is missing or invalid");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostTaskListDto dto)
    {
        var userId = GetCurrentUserId();
        var id = await _taskListService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        var result = await _taskListService.GetAllAsync(userId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _taskListService.GetByIdAsync(id, userId);
        if (result == null)
            return NotFound();

        return Ok(result);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PutTaskListDto dto)
    {
        var userId = GetCurrentUserId();
        var success = await _taskListService.UpdateAsync(id, dto, userId);
        return success ? NoContent() : Forbid();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        var success = await _taskListService.DeleteAsync(id, userId);
        return success ? NoContent() : Forbid();
    }
}