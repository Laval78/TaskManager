using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTO.User;
using TaskManager.Application.Interfaces.Service;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Створює нового користувача
        /// </summary>
        /// <param name="dto">Дані нового користувача</param>
        /// <returns>Повідомлення про успішне створення</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] PostUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.CreateAsync(dto);
            return Ok(new { message = "Користувач успішно створений" });
        }
    }
}