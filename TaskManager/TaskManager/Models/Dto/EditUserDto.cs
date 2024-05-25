namespace TaskManager.Models.Dto;

public class EditUserDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IFormFile? Avatar { get; set; }
}