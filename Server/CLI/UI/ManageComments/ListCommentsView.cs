using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;

    public ListCommentsView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public Task ListAllCommentsAsync()
    {
        List<Comment> comments = commentRepository.GetMany().ToList();
        if (!comments.Any())
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("No comments found.");
            return Task.CompletedTask;
        }

        Console.WriteLine("----------------------");
        foreach (Comment comment in comments)
        {
            Console.WriteLine($"[{comment.CommentId}] on post {comment.PostId} by user {comment.UserId}: {comment.Body}");
        }

        return Task.CompletedTask;
    }
}
