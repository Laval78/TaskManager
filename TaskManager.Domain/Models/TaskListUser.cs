namespace TaskManager.Domain.Models
{
    public class TaskListUser
    {
        public int Id { get; set; }

        public int IdTask { get; set; }

        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; }

        public virtual TaskList IdTaskListNavigation { get; set; }

        public TaskListUser(int Id, int IdTask, int IdUser)
        {
            this.Id = Id;
            this.IdTask = IdTask;
            this.IdUser = IdUser;
        }

        public TaskListUser()
        {
        }
    }
}
