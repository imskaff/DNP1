using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public PostsController(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> AddPost([FromBody] CreatePostDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Title or body is empty");
        }

        if (!UserExists(request.UserId))
        {
            return BadRequest($"User with ID '{request.UserId}' does not exist");
        }

        Post created = await postRepository.AddAsync(new Post(request.Title, request.Body, request.UserId));
        PostDTO dto = ToDto(created);
        return Created($"/Posts/{dto.Id}", dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDTO request)
    {
        Post existing = await postRepository.GetSingleAsync(id);

        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Title or body is empty");
        }

        existing.Title = request.Title;
        existing.Body = request.Body;
        await postRepository.UpdateAsync(existing);
        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDTO>> GetSingle([FromRoute] int id)
    {
        Post post = await postRepository.GetSingleAsync(id);
        return Ok(ToDto(post));
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDTO>> GetMany([FromQuery] string? title, [FromQuery] int? userId, [FromQuery] string? userName)
    {
        IQueryable<Post> posts = postRepository.GetMany();
        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.ToLower().Contains(title.ToLower()));
        }

        if (userId is not null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            List<int> userIds = GetUserIdsByName(userName);
            posts = posts.Where(p => userIds.Contains(p.UserId));
        }

        List<PostDTO> dtobjs = posts.Select(p => ToDto(p)).ToList();
        return Ok(dtobjs);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        await postRepository.DeleteAsync(id);
        return NoContent();
    }

    private bool UserExists(int userId)
    {
        return userRepository.GetMany().Any(u => u.UserId == userId);
    }

    private List<int> GetUserIdsByName(string userName)
    {
        return userRepository.GetMany()
            .Where(u => u.Username.ToLower().Contains(userName.ToLower()))
            .Select(u => u.UserId)
            .ToList();
    }

    private string GetAuthorName(int userId)
    {
        User? author = userRepository.GetMany().SingleOrDefault(u => u.UserId == userId);
        if (author is null)
        {
            return "[deleted]";
        }
        return author.Username;
    }

    private PostDTO ToDto(Post post) => new PostDTO()
    {
        Id = post.PostId,
        Title = post.Title,
        Body = post.Body,
        UserId = post.UserId,
        AuthorName = GetAuthorName(post.UserId)
    };
}