namespace TaskManager.Models.Entity;

public class UserToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime TokenExp { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }
}