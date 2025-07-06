using TaskManager.Application.DTO.TaskList;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Mappers
{
    public static class TaskListMapper
    {
        /// <summary>
        /// Мапінг Post DTO у TaskList модель (створення нового списку задач)
        /// </summary>
        public static TaskList ToTaskList(this PostTaskListDto dto, int whoCreated)
        {
            return new TaskList
            {
                Title = dto.Title,
                WhoCreated = whoCreated,
                DateTimeCreated = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Оновлює TaskList згідно з Put DTO
        /// </summary>
        public static void MapUpdateFromDto(this TaskList taskList, PutTaskListDto dto)
        {
            taskList.Title = dto.Title;
        }

        /// <summary>
        /// Мапінг TaskList у DTO для перегляду одного списку задач
        /// </summary>
        public static GetTaskListDto ToGetTaskListDto(this TaskList taskList)
        {
            return new GetTaskListDto
            {
                Id = taskList.Id,
                Title = taskList.Title,
                DataTimeCreated = taskList.DateTimeCreated,
                WhoCreated = taskList.WhoCreatedNavigation?.Name ?? "Невідомо"
            };
        }

        /// <summary>
        /// Мапінг TaskList у DTO для списку списків задач (спрощений перегляд)
        /// </summary>
        public static GetTaskListForListDto ToGetTaskListForListDto(this TaskList taskList)
        {
            return new GetTaskListForListDto
            {
                Id = taskList.Id,
                Name = taskList.Title
            };
        }
    }
}