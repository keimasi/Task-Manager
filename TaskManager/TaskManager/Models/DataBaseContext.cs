using Microsoft.EntityFrameworkCore;
using TaskManager.Models.Entity;
using Task = TaskManager.Models.Entity.Task;

namespace TaskManager.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserProject>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });

            modelBuilder.Entity<UserProject>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<UserProject>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(pu => pu.UserId);

            //*******************************************

            // رابطه یک به چند بین Comment و User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // یا DeleteBehavior.NoAction

            // رابطه یک به چند بین Comment و Task
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade); // یا DeleteBehavior.NoAction

            //*******************************************

            //رابطه یک به چند پیام به کاربر
            modelBuilder.Entity<Chat>()
                .HasOne(x => x.User)
                .WithMany(x => x.Chats)
                .HasForeignKey(x => x.UserID);
            
            //رابطه یک به چند پیام به پروژه
            modelBuilder.Entity<Chat>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Chats)
                .HasForeignKey(x => x.ProjectId);

            //*******************************************


            //رابطه یک به چند وظیفه به کاربر
            modelBuilder.Entity<Task>()
                .HasOne(x => x.User)
                .WithMany(x => x.Task);
                
            
            // //رابطه یک به چند وظیفه به پروژه
            modelBuilder.Entity<Task>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Tasks);
                
            
            //*******************************************
            
            //رابطه چند به چند پروژه و کاربر
            modelBuilder.Entity<Project>()
                .HasMany(x => x.Users)
                .WithMany(x => x.Projects);

        }

    }
}
