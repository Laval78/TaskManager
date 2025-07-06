using TaskManager.Domain.Models;

public interface IUsersRepository
{
    Task AddAsync(User user);
}