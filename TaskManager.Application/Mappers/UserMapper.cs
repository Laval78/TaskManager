using TaskManager.Application.DTO.User;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Mappers
{
    public static class UserMapper
    {
        /// <summary>
        /// Мапінг Post DTO до моделі User (створення користувача)
        /// </summary>
        public static User ToPostUserDto(PostUserDto dto)
        {
            return new User
            {
                Name = dto.Name
            };
        }
    }
}