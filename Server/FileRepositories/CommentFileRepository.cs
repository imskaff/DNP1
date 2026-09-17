using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filepath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        comment.CommentId = comments.Any() ? comments.Max(c => c.CommentId) + 1 : 1;
        comments.Add(comment);
        await SaveCommentsAsync(comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? existingComment = comments.SingleOrDefault(c =>
            c.CommentId == comment.CommentId);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.CommentId}' not found");
        }
        
        int index = comments.IndexOf(existingComment);
        comments[index] = comment;
        await SaveCommentsAsync(comments);
    }
    
    public async Task DeleteAsync(int id)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? commentToRemove = comments.SingleOrDefault(c => c.CommentId == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        await SaveCommentsAsync(comments);
    }

    public async Task<Comment> GetSingleAsync(int commentId)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? comment = comments.SingleOrDefault(c => c.CommentId == commentId);
        if (comment is null) 
        {
            throw new InvalidOperationException(
                $"Comment with ID '{commentId}' not found");
        }
        return comment;
    }

    public IQueryable<Comment> GetMany() {
        string commentsAsJson = File.ReadAllTextAsync(filepath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }
    
    private async Task<List<Comment>> LoadCommentsAsync()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
    }

    private async Task SaveCommentsAsync(List<Comment> comments)
    {
        string commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filepath, commentsAsJson);
    }
}