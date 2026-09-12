using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    // private readonly IUserRepository userRepository;
    private readonly CreateUserView createUserView;
    private readonly ListUsersView listUsersView;
    private readonly DeleteUserView deleteUserView;
    private readonly UpdateUserView updateUserView;

    public ManageUsersView(IUserRepository userRepository)
    {
        // this.userRepository = userRepository;
        createUserView = new CreateUserView(userRepository);
        listUsersView = new ListUsersView(userRepository);
        deleteUserView = new DeleteUserView(userRepository);
        updateUserView = new UpdateUserView(userRepository);
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("=== Manage Users ===");
            Console.WriteLine("1. Create new user");
            Console.WriteLine("2. See all users");
            Console.WriteLine("3. Update user");
            Console.WriteLine("4. Delete user");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Select an option: ");

            string? choice = Console.ReadLine();
            if (choice is null)
            {
                return;
            }

            try
            {
                switch (choice)
                {
                    case "1":
                        await createUserView.AddUserAsync();
                        break;
                    case "2":
                        await listUsersView.ListAllUsersAsync();
                        break;
                    case "3":
                        await updateUserView.UpdateUserAsync();
                        break;
                    case "4":
                        await deleteUserView.DeleteUserAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
        }
    }
}
