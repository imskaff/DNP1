using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UpdateUserView
{
    private readonly IUserRepository userRepository;

    public UpdateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task UpdateUserAsync()
    {
        Console.Write("Enter user ID to update: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || int.Parse(input) < 0)
        {
            throw new Exception("Invalid ID");
        }

        int id = int.Parse(input);
        User? userToUpdate = userRepository.GetMany().FirstOrDefault(u => u.UserId == id);
        if (userToUpdate == null)
        {
            throw new Exception("This ID does not exist");
        }

        Console.Write($"Enter new username (or click Enter to keep current: {userToUpdate.Username}): ");
        string? newUsername = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newUsername))
        {
            if (userRepository.GetMany().Any(u => u.Username == newUsername))
            {
                Console.WriteLine("Username is already taken");
            }
            else
            {
                userToUpdate.Username = newUsername;
            }
        }
        
        Console.Write($"Enter new password (or click Enter to keep current: {userToUpdate.Password}): ");
        string? newPassword = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            userToUpdate.Password = newPassword;
        }

        await userRepository.UpdateAsync(userToUpdate);
        Console.WriteLine($"User {id} updated.");
    }
}