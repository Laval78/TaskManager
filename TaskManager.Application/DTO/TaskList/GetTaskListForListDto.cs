namespace TaskManager.Application.DTO.TaskList
{
    /// <summary>
    /// DTO виводу списку спискiв задач 
    /// </summary>
    public class GetTaskListForListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
