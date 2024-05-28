﻿using System;

namespace TaskManager.Models.Entity
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<User> Users { get; set; }
        // public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        // public Task? Tasks { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Chat> Chats { get; set; }


        public Project(string name, string description, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            UserProjects = new List<UserProject>();
        }
    }
}
