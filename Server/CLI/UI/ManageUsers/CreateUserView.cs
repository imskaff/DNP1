using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task AddUserAsync()
    {
        string username;
        while (true)
        {
            Console.Write("Enter username: ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                throw new Exception("Input ended unexpectedly");
            }
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Username cannot be empty");
            }
            else if (userRepository.GetMany().Any(u => u.Username == input))
            {
                Console.WriteLine("Username is already taken");
            }
            else
            {
                username = input;
                break;
            }
        }

        string password;
        while (true)
        {
            Console.Write("Enter password: ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                throw new Exception("Input ended unexpectedly");
            }
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Password cannot be empty");
            }
            else
            {
                password = input;
                break;
            }
        }

        User user = new User(username, password);

        User created = await userRepository.AddAsync(user);
        Console.WriteLine($"User created with ID: {created.UserId}");
    }
}
