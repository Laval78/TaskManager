using TaskManager.Application.DTO.User;

namespace TaskManager.Application.Interfaces.Service
{
    /// <summary>
    /// Інтерфейс сервісу користувачів
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Створити нового користувача
        /// </summary>
        Task CreateAsync(PostUserDto dto);
    }
}