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
        private readonly IUserRepository _userRepository;
        private readonly ILoggerManager _logger;

        public UserService(IUserRepository userRepository, ILoggerManager logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Створення нового користувача
        /// </summary>
        public async Task CreateAsync(PostUserDto dto)
        {
            _logger.LogInfo($"Спроба створити користувача з іменем '{dto.Name}'");

            var user = new User
            {
                Name = dto.Name
            };

            await _userRepository.AddAsync(user);

            _logger.LogInfo($"Користувача '{dto.Name}' створено успішно");
        }
    }
}