using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentsView
{
    private readonly CreateCommentView createCommentView;
    private readonly ListCommentsView listCommentsView;

    public ManageCommentsView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        createCommentView = new CreateCommentView(commentRepository, postRepository, userRepository);
        listCommentsView = new ListCommentsView(commentRepository);
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage Comments ===");
            Console.WriteLine("1. Add comment to a post");
            Console.WriteLine("2. See all comments");
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
                        await createCommentView.AddCommentAsync();
                        break;
                    case "2":
                        await listCommentsView.ListAllCommentsAsync();
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
