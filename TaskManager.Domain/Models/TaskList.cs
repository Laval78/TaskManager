using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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

        public TaskList(int Id, string Title, DateTime DateTimeCreated, int WhoCreated)
        {
            this.Id = Id;
            this.Title = Title;
            this.DateTimeCreated = DateTimeCreated;
            this.WhoCreated = WhoCreated;
        }

        public TaskList()
        {
        }
    }
}
