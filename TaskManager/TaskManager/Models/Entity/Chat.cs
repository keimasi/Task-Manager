namespace TaskManager.Models.Entity
{
    public class Chat
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserID { get; set; }
        public User User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public Chat(string text, int userID, int projectId)
        {
            Text = text;
            UserID = userID;
            ProjectId = projectId;
        }
    }
}
