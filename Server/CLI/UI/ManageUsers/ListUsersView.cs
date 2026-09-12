using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public Task ListAllUsersAsync()
    {
        List<User> users = userRepository.GetMany().ToList();
        if (!users.Any())
        {
            Console.WriteLine("No users found.");
            return Task.CompletedTask;
        }

        Console.WriteLine("--- Users ---");
        foreach (User user in users)
        {
            Console.WriteLine($"{user.UserId}: {user.Username}");
        }

        return Task.CompletedTask;
    }
}
