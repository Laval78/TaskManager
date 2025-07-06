using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTO.TaskList
{
    /// <summary>
    /// DTO для оновлення назви списку задач
    /// </summary>
    public class PutTaskListDto
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; } = null!;
    }
}
