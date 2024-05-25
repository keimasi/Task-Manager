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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //رابطه یک به جند کامنت و کاربران
            modelBuilder.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserID);
            
            //رابطه یک به چند کامنت به پروژه
            modelBuilder.Entity<Comment>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ProjectId);

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
                .WithOne(x => x.Tasks);
                
            
            //*******************************************
            
            //رابطه چند به چند پروژه و کاربر
            modelBuilder.Entity<Project>()
                .HasMany(x => x.Users)
                .WithMany(x => x.Projects);
            

        }

    }
}
