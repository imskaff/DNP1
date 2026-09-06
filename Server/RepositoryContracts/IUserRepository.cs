using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int userId);
    
    // applicable for User?
    // Task<User> GetSingleAsync(int userId);
    // IQueryable<User> GetMany();
}