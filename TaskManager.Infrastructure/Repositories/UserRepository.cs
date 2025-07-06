using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly TaskManagerContext _context;


    public UsersRepository(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}