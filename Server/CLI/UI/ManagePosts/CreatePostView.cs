using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    
    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task AddPostAsync()
    {
        string title;
        while (true)
        {
            Console.WriteLine("Enter the title: ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                throw new Exception("Input ended unexpectedly");
            }
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Title cannot be empty");
            }
            else
            {
                title = input;
                break;
            }
        }

        string body;
        while (true)
        {
            Console.WriteLine("Enter the body: ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                throw new Exception("Input ended unexpectedly");
            }
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Body cannot be empty");
            }
            else
            {
                body = input;
                break;
            }
        }

        Console.WriteLine("Enter author user ID: ");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrEmpty(idInput) || int.Parse(idInput) < 0)
        {
            throw new Exception("Invalid ID");
        }

        int id = int.Parse(idInput);
        if (!userRepository.GetMany().Any(u => u.UserId == id))
        {
            throw new Exception("This user ID does not exist");
        }

        Post post = new Post(title, body, id);

        Post created = await postRepository.AddAsync(post);
        Console.WriteLine($"Post created with ID: {created.PostId}");
    }
}