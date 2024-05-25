namespace TaskManager.Models.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        
        
        public IEnumerable<Task>? Task { get;  set; }
        public ICollection<Comment> Comments { get;  set; }
        public ICollection<Project> Projects { get;  set; }
        public ICollection<Chat> Chats { get;  set; }
        public ICollection<UserToken> Tokens { get; set; }
        

        
        public User(string? userName, string? firstName, string lastName, string password, string avatar)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Avatar = avatar;
            IsActive = true;
        }

        public void Edit(string? userName, string? firstName, string lastName, string password, string avatar)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Avatar = avatar;
        }
        
        public void ChangePassword(string password)
        {
            Password = password;
        }
    }
}
