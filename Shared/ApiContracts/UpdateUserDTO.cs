namespace ApiContracts;

public class UpdateUserDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}