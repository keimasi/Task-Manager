namespace TaskManager.Models.Dto;

public class RegisterUser
{
    /// <summary>
    /// نام کاربری
    /// </summary> 
    public string UserName { get; set; }
    /// <summary>
    /// نام
    /// </summary> 
    public string FirstName { get; set; }
    /// <summary>
    /// رمز عبور
    /// </summary> 
    public string Password { get; set; }
    /// <summary>
    /// تایید رمز عبور
    /// </summary> 
    public string PasswordConfirm { get; set; }
}