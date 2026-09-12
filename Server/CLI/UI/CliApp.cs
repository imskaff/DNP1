using CLI.UI.ManageUsers;
using CLI.UI.ManagePosts;
using CLI.UI.ManageComments;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        await StartMainMenu();
        Console.WriteLine("Exiting...");
    }

    private async Task StartMainMenu()
    {
        ManageUsersView manageUsersView = new ManageUsersView(userRepository);
        ManagePostsView managePostsView = new ManagePostsView(postRepository, userRepository, commentRepository);
        ManageCommentsView manageCommentsView = new ManageCommentsView(commentRepository, postRepository, userRepository);

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Manage users");
            Console.WriteLine("2. Manage posts");
            Console.WriteLine("3. Manage comments");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            string? choice = Console.ReadLine();
            if (choice is null)
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    await manageUsersView.ShowMenuAsync();
                    break;
                case "2":
                    await managePostsView.ShowMenuAsync();
                    break;
                case "3":
                    await manageCommentsView.ShowMenuAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
