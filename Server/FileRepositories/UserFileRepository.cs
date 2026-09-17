using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filepath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await LoadUsersAsync();
        user.UserId = users.Any() ? users.Max(u => u.UserId) + 1 : 1;
        users.Add(user);
        await SaveUsersAsync(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await LoadUsersAsync();
        User? existingUser = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.UserId}' not found");
        }
        
        int index = users.IndexOf(existingUser);
        users[index] = user;
        await SaveUsersAsync(users);
    }

    public async Task DeleteAsync(int userId)
    {
        List<User> users = await LoadUsersAsync();
        User? userToRemove = users.SingleOrDefault(u => u.UserId == userId);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{userId}' not found");
        }
        
        users.Remove(userToRemove);
        await SaveUsersAsync(users);
    }

    public async Task<User> GetSingleAsync(int userId)
    {
        List<User> users = await LoadUsersAsync();
        User? user = users.SingleOrDefault(u => u.UserId == userId);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{userId}' not found");
        }
        return user;
    }

    public IQueryable<User> GetMany() {
        string usersAsJson = File.ReadAllTextAsync(filepath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }

    private async Task<List<User>> LoadUsersAsync()
    {
        string usersAsJson = await File.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filepath, usersAsJson);
    }
}