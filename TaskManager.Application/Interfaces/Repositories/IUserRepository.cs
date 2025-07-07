using TaskManager.Domain.Models;

public interface IUserRepository
{
    Task AddAsync(User user);
}