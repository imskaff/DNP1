using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments;

    public CommentInMemoryRepository()
    {
        comments = new List<Comment>
        {
            new Comment("Hi everyone, glad to be here.", 1, 2) { CommentId = 1 },
            new Comment("await unwraps the value inside the Task.", 2, 1) { CommentId = 2 },
            new Comment("That finally made it click, thanks.", 2, 3) { CommentId = 3 },
            new Comment("Upgrade was painless for me.", 3, 2) { CommentId = 4 }
        };
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.CommentId = comments.Any()
            ? comments.Max(c => c.CommentId) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }
    
    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.CommentId == comment.CommentId);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{comment.CommentId}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(c => c.CommentId == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(c => c.CommentId == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        return Task.FromResult(comment);
    }
    
    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}