namespace TaskManager.Models.Dto;

public class CreateUserDto
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public IFormFile? Avatar { get; set; }
}