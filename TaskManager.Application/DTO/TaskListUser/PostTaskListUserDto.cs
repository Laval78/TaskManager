using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTO.TaskListUser
{
    /// <summary>
    /// DTO для зв'язку користувача з списоком задач
    /// </summary>
    public class PostTaskListUserDto
    {
        [Required]
        public int UserId { get; set; }
    }
}