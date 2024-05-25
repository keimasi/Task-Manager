namespace TaskManager.Models.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        public int UserID { get; set; }
        public int ProjectId { get; set; }
        public User User { get; set; }
        public Project Project { get; set; }

        public Comment(string title, string text, int userID, int projectId)
        {
            Title = title;
            Text = text;
            UserID = userID;
            ProjectId = projectId;
        }
    }
}
