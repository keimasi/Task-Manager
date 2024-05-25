namespace TaskManager.Models.Dto;

public class ChatViewModel
{
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int UserID { get; set; }
    public int ProjectId { get; set; }
}