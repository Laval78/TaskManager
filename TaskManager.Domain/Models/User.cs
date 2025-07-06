namespace TaskManager.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TaskList> TaskLists { get; set; }
        public virtual ICollection<TaskListUser> TaskListUsers { get; set; }

        public User(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public User() 
        {
        }
    }
}
