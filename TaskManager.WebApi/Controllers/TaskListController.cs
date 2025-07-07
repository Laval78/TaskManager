using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTO.TaskList;
using TaskManager.Application.Interfaces.Service;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/tasklists")]
    [RequireUserIdHeader]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskListService _taskListService;

        public TaskListController(ITaskListService taskListService)
        {
            _taskListService = taskListService;
        }

        private int UserId => HttpContext.Items.TryGetValue("UserId", out var value) && value is int id
            ? id
            : throw new UnauthorizedAccessException("User ID is missing from request context.");

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostTaskListDto dto)
        {
            var id = await _taskListService.CreateAsync(dto, UserId);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _taskListService.GetAllAsync(UserId, page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskListService.GetByIdAsync(id, UserId);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutTaskListDto dto)
        {
            await _taskListService.UpdateAsync(id, dto, UserId);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskListService.DeleteAsync(id, UserId);
            return NoContent();
        }
    }
}

