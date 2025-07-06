using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.Models
{
    public class TaskList
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public int WhoCreated { get; set; }

        public virtual User WhoCreatedNavigation { get; set; }

        public virtual ICollection<TaskListUser> TaskListUsers { get; set; }
    }
}
