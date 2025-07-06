namespace TaskManager.Application.DTO.User
{
    /// <summary>
    /// DTO для отримання користувача 
    /// </summary>
    public class GetUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}