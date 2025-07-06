namespace TaskManager.Application.DTO.TaskList
{
    /// <summary>
    /// DTO для отримення одного списку задач
    /// </summary>
    public class GetTaskListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime DataTimeCreated { get; set; }

        public string WhoCreated { get; set; } = null!;
    }
}
