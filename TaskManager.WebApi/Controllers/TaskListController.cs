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

        /// <summary>
        /// Створює новий список задач
        /// </summary>
        /// <param name="dto">Об'єкт з назвою списку задач</param>
        /// <returns>Інформація про створений список задач</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostTaskListDto dto)
        {
            var id = await _taskListService.CreateAsync(dto, UserId);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }


        /// <summary>
        /// Отримує всі списки задач користувача з пагінацією
        /// </summary>
        /// <param name="page">Номер сторінки</param>
        /// <param name="pageSize">Кількість елементів на сторінці</param>
        /// <returns>Списки задач користувача</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _taskListService.GetAllAsync(UserId, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Отримує конкретний список задач за його ID
        /// </summary>
        /// <param name="id">ID списку задач</param>
        /// <returns>Конкретний список задач</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskListService.GetByIdAsync(id, UserId);
            return Ok(result);
        }


        /// <summary>
        /// Оновлює список задач
        /// </summary>
        /// <param name="id">ID списку задач</param>
        /// <param name="dto">Нові дані для оновлення</param>
        /// <returns>Пустий результат при успішному оновленні</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutTaskListDto dto)
        {
            await _taskListService.UpdateAsync(id, dto, UserId);
            return NoContent();
        }

        /// <summary>
        /// Видаляє список задач
        /// </summary>
        /// <param name="id">ID списку задач</param>
        /// <returns>Підтвердження успішного видалення</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskListService.DeleteAsync(id, UserId);
            return NoContent();
        }
    }
}

