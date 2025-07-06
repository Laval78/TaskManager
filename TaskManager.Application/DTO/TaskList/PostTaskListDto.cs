using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTO.TaskList
{
    /// <summary>
    /// DTO для створення списку задач
    /// </summary>
    public class PostTaskListDto
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; } = null!;
    }
}
