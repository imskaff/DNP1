using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;

    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    public async Task ViewPostAsync()
    {
        Console.Write("Enter post ID to view: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || int.Parse(input) < 0)
        {
            throw new Exception("Invalid ID");
        }

        int postId = int.Parse(input);
        Post post = await postRepository.GetSingleAsync(postId);

        Console.WriteLine();
        Console.WriteLine($"--- Post {post.PostId} ---");
        Console.WriteLine($"Title:  {post.Title}");
        Console.WriteLine($"Author: {GetUsername(post.UserId)}");
        Console.WriteLine($"Body:   {post.Body}");
        Console.WriteLine();

        List<Comment> comments = commentRepository.GetMany()
            .Where(c => c.PostId == postId)
            .ToList();

        if (!comments.Any())
        {
            Console.WriteLine("No comments on this post.");
            return;
        }

        Console.WriteLine($"{comments.Count} comment(s):");
        foreach (Comment comment in comments)
        {
            Console.WriteLine(
                    $"  [{comment.CommentId}] {GetUsername(comment.UserId)}: {comment.Body}");
        }
    }

    private string GetUsername(int userId)
    {
        User? user = userRepository.GetMany().FirstOrDefault(u => u.UserId == userId);
        if (user is null)
        {
            return $"Deleted user {userId}";
        }
        return user.Username;
    }
}
