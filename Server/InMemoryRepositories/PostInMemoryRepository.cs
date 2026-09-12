using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts;

    public PostInMemoryRepository()
    {
        posts = new List<Post>
        {
            new Post("Welcome to the forum", "Introduce yourself here!", 1) { PostId = 1 },
            new Post("Async in C#", "Can someone explain Task<T> vs Task?", 2) { PostId = 2 },
            new Post("Anyone on .NET 10?", "Curious how the upgrade went for you.", 1) { PostId = 3 },
            new Post("Repository pattern tips", "Interfaces first, implementations later.", 3) { PostId = 4 }
        };
    }

    public Task<Post> AddAsync(Post post)
    {
        post.PostId = posts.Any()
            ? posts.Max(p => p.PostId) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }
    
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.PostId}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Post> GetSingleAsync(int id)
    {
        // Do implementation
        Post? post = posts.SingleOrDefault(p => p.PostId == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        
        return Task.FromResult(post);
    }
    
    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}