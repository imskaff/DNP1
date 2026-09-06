using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(int commentId);
    
    // applicable for Comment?
    // Task<Comment> GetSingleAsync(int commentId);
    
    IQueryable<Comment> GetMany();
}