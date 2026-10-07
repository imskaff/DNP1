using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;
    
    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] CreateUserDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username or password is empty");
        }

        if (IsUserNameTaken(request.UserName))
        {
            return BadRequest("Username is taken");
        }
        
        User created = await userRepository.AddAsync(new User(request.UserName, request.Password));
        UserDTO dto = ToDto(created);
        return Created($"/Users/{dto.Id}", dto);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDTO request)
    {
        User existing = await userRepository.GetSingleAsync(id);

        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password is empty");
        }
        
        if (IsUserNameTaken(request.UserName, exceptUserId: id))
        {
            return BadRequest($"Username '{request.UserName}' is already taken");
        }

        existing.Username = request.UserName;
        existing.Password = request.Password;
        await userRepository.UpdateAsync(existing);
        return NoContent();
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDTO>> GetSingle([FromRoute] int id)
    {
        User user = await userRepository.GetSingleAsync(id);
        return Ok(ToDto(user));
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDTO>> GetMany([FromQuery] string? userName)
    {
        IQueryable<User> users = userRepository.GetMany();
        if (!string.IsNullOrWhiteSpace(userName))
        {
            users = users.Where(u => u.Username.ToLower().Contains(userName.ToLower()));
        }

        List<UserDTO> dtobjs = users.Select(u => ToDto(u)).ToList();
        return Ok(dtobjs);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        await userRepository.DeleteAsync(id);
        return NoContent();
    }

    private bool IsUserNameTaken(string userName, int? exceptUserId = null)
    {
        return userRepository.GetMany().Any(u => u.Username == userName && u.UserId != exceptUserId);
    }
    
    private static UserDTO ToDto(User user) => new UserDTO()
    {
        Id = user.UserId,
        UserName = user.Username
    };
}