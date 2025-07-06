using TaskManager.Application.DTO.User;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Services
{
    /// <summary>
    /// Сервіс для роботи з користувачами (мінімальний функціонал — лише створення)
    /// </summary>
    public class UserService : IUserService
    {
        private readonly TaskManagerContext _context;

        public UserService(TaskManagerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Створення нового користувача
        /// </summary>
        public async Task CreateAsync(PostUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}