using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly CreatePostView createPostView;
    private readonly ListPostsView listPostsView;
    private readonly SinglePostView singlePostView;

public ManagePostsView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
{
    createPostView = new CreatePostView(postRepository, userRepository);
    listPostsView = new ListPostsView(postRepository);
    singlePostView = new SinglePostView(postRepository, commentRepository, userRepository);
}


    public async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("=== Manage Posts ===");
            Console.WriteLine("1. Create new post");
            Console.WriteLine("2. See all posts");
            Console.WriteLine("3. See a single post");
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
                        await createPostView.AddPostAsync();
                        break;
                    case "2":
                        await listPostsView.ListAllPostsAsync();
                        break;
                    case "3":
                        await singlePostView.ViewPostAsync();
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
