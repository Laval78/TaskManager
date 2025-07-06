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
        /// Створення нового користувача
        /// </summary>
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