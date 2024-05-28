namespace TaskManager.Models.Entity;

public class UserProject
{
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}