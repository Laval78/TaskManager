using System;
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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            await _userRepository.AddAsync(user);
        }
    }
}