using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users;

    public UserInMemoryRepository()
    {
        users = new List<User>
        {
            new User("alice", "pass123") { UserId = 1 },
            new User("bob", "hunter2") { UserId = 2 },
            new User("charlie", "letmein") { UserId = 3 }
        };
    }
    
    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Any()
            ? users.Max(u => u.UserId) + 1
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
    
    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(u => u.UserId == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        return Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}