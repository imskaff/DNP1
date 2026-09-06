using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users;
    
    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Any()
            ? users.Max(p => p.UserId) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }
    
    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.UserId}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int userId)
    {
        User? userToDelete = users.SingleOrDefault(u => u.UserId == userId);
        if (userToDelete is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{userId}' not found");
        }

        users.Remove(userToDelete);
        
        return Task.CompletedTask;
    }
}