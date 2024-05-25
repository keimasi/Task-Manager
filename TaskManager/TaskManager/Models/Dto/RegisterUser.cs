namespace TaskManager.Models.Dto;

public class RegisterUser
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}