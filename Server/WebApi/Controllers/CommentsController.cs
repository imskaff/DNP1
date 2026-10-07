using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CommentsController(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDTO>> AddComment([FromBody] CreateCommentDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Comment body is empty");
        }

        if (!PostExists(request.PostId))
        {
            return BadRequest($"Post with ID '{request.PostId}' does not exist");
        }

        if (!UserExists(request.UserId))
        {
            return BadRequest($"User with ID '{request.UserId}' does not exist");
        }

        Comment created = await commentRepository.AddAsync(new Comment(request.Body, request.PostId, request.UserId));
        CommentDTO dto = ToDto(created);
        return Created($"/Comments/{dto.Id}", dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDTO request)
    {
        Comment existing = await commentRepository.GetSingleAsync(id);

        if (string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Comment body is empty");
        }

        existing.Body = request.Body;
        await commentRepository.UpdateAsync(existing);
        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDTO>> GetSingle([FromRoute] int id)
    {
        Comment comment = await commentRepository.GetSingleAsync(id);
        return Ok(ToDto(comment));
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDTO>> GetMany([FromQuery] int? postId, [FromQuery] int? userId, [FromQuery] string? userName)
    {
        IQueryable<Comment> comments = commentRepository.GetMany();
        if (postId is not null)
        {
            comments = comments.Where(c => c.PostId == postId);
        }

        if (userId is not null)
        {
            comments = comments.Where(c => c.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            List<int> userIds = GetUserIdsByName(userName);
            comments = comments.Where(c => userIds.Contains(c.UserId));
        }

        List<CommentDTO> dtobjs = comments.Select(c => ToDto(c)).ToList();
        return Ok(dtobjs);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        await commentRepository.DeleteAsync(id);
        return NoContent();
    }

    private bool PostExists(int postId)
    {
        return postRepository.GetMany().Any(p => p.PostId == postId);
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

    private CommentDTO ToDto(Comment comment) => new CommentDTO()
    {
        Id = comment.CommentId,
        Body = comment.Body,
        PostId = comment.PostId,
        UserId = comment.UserId,
        AuthorName = GetAuthorName(comment.UserId)
    };
}
