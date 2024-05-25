namespace TaskManager.Models.Dto;

public class CreateUserDto
{
    /// <summary>
    /// نام کاربری
    /// </summary> 
    public string? UserName { get; set; }
    /// <summary>
    /// نام
    /// </summary> 
    public string? FirstName { get; set; }
    /// <summary>
    /// نام خانوادگی
    /// </summary> 
    public string? LastName { get; set; }
    /// <summary>
    /// رمز عبور
    /// </summary> 
    public string? Password { get; set; }
    
    public IFormFile? Avatar { get; set; }
}