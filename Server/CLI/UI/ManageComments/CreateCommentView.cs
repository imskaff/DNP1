using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreateCommentView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task AddCommentAsync()
    {
        Console.Write("Enter post ID to comment on: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || int.Parse(input) < 0)
        {
            throw new Exception("Invalid post ID");
        }

        int postId = int.Parse(input);
        if (!postRepository.GetMany().Any(p => p.PostId == postId))
        {
            throw new Exception("This post ID does not exist");
        }

        Console.Write("Enter author user ID: ");
        string? userIdInput = Console.ReadLine();
        if (string.IsNullOrEmpty(userIdInput) || int.Parse(userIdInput) < 0)
        {
            throw new Exception("Invalid user ID");
        }

        int userId = int.Parse(userIdInput);
        if (!userRepository.GetMany().Any(u => u.UserId == userId))
        {
            throw new Exception("This user ID does not exist");
        }

        string body;
        while (true)
        {
            Console.Write("Enter comment body: ");
            input = Console.ReadLine();
            if (input is null)
            {
                throw new Exception("Input ended unexpectedly");
            }
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Comment body cannot be empty");
            }
            else
            {
                body = input;
                break;
            }
        }

        Comment comment = new Comment(body, postId, userId);

        Comment created = await commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment created with ID: {created.CommentId}");
    }
}
