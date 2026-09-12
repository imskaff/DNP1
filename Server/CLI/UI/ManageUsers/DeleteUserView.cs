using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class DeleteUserView
{
    private readonly IUserRepository userRepository;

    public DeleteUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task DeleteUserAsync()
    {
        Console.Write("Enter user ID to Delete: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || int.Parse(input) < 0)
        {
            throw new Exception("Invalid ID");
        }

        int id = int.Parse(input);
        if (!userRepository.GetMany().Any(u => u.UserId == id))
        {
            throw new Exception("This ID does not exist");
        }

        await userRepository.DeleteAsync(id);
        Console.WriteLine($"User {id} deleted.");
    }
}