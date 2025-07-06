using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Models;
using TaskManager.Infrastructure;

public class TaskListRepository : ITaskListRepository
{
    private readonly TaskManagerContext _context;


    public TaskListRepository(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(TaskList taskList)
    {
        _context.TaskLists.Add(taskList);
        await _context.SaveChangesAsync();
        return taskList.Id;
    }

    public async Task<IEnumerable<TaskList>> GetAllForUserAsync(int userId, int page, int pageSize)
    {
        return await _context.TaskLists
            .Where(t => t.WhoCreated == userId || t.TaskListUsers.Any(x => x.IdUser == userId))
            .OrderByDescending(t => t.DateTimeCreated)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<TaskList?> GetByIdWithCreatorAsync(int id, int userId)
    {
        return await _context.TaskLists
            .Include(t => t.WhoCreatedNavigation)
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                (t.WhoCreated == userId || t.TaskListUsers.Any(x => x.IdUser == userId)));
    }

    public async Task<TaskList?> GetByIdAsync(int id)
    {
        return await _context.TaskLists.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(TaskList taskList)
    {
        _context.TaskLists.Remove(taskList);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}