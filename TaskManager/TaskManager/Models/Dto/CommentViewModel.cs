using TaskManager.Models.Entity;

namespace TaskManager.Models.Dto;

public class CommentViewModel
{
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int UserID { get; set; }
    public int ProjectId { get; set; }
    public User User { get; set; }
    public Project Project { get; set; }
}