namespace TaskManager.Models.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User User { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }

        public Comment(string title, string text, int userId, int taskId)
        {
            Title = title;
            Text = text;
            UserId = userId;
            TaskId = taskId;
        }
    }
}
