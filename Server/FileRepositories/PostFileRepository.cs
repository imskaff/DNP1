using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filepath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        post.PostId = posts.Any() ? posts.Max(x => x.PostId) + 1 : 1;
        posts.Add(post);
        await SavePostsAsync(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? existingPost = posts.SingleOrDefault(x => x.PostId == post.PostId);
        if (existingPost == null)
        {
            throw new InvalidOperationException(
                $"Post with id '{post.PostId}' not found");
        }
        
        int index = posts.IndexOf(existingPost);
        posts[index] = post;
        await SavePostsAsync(posts);
    }

    public async Task DeleteAsync(int postId)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? postToRemove = posts.SingleOrDefault(x => x.PostId == postId);
        if (postToRemove == null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{postId}' not found");
        }
        
        posts.Remove(postToRemove);
        await SavePostsAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int postId)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? post = posts.SingleOrDefault(x => x.PostId == postId);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{postId}' not found");
        }
        return post;
    }
    
    public IQueryable<Post> GetMany() {
        string postsAsJson = File.ReadAllTextAsync(filepath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }

    private async Task<List<Post>> LoadPostsAsync()
    {
        string postsAsJson = await File.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
    }

    private async Task SavePostsAsync(List<Post> posts)
    {
        string postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filepath, postsAsJson);
    }
}