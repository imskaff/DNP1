using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public Task ListAllPostsAsync()
    {
        List<Post> posts = postRepository.GetMany().ToList();
        if (!posts.Any())
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("No posts found.");
            return Task.CompletedTask;
        }

        Console.WriteLine("----------------------");
        foreach (Post post in posts)
        {
            Console.WriteLine($"[{post.Title}, {post.PostId}]");
        }

        return Task.CompletedTask;
    }
}
