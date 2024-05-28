﻿using TaskManager.Models.Entity;

namespace TaskManager.Models.Dto
{
    public class CreateProjectDto
    {
        /// <summary>
        ///نام پروژه
        /// </summary> 
        public string Name { get; set; }
        /// <summary>
        ///توضیحات پروژه
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        ///زمان شروع پروژه
        /// </summary> 
        public DateTime StartTime { get; set; }
        /// <summary>
        ///زمان پایان پروژه
        /// </summary> 
        public DateTime EndTime { get; set; }
        public List<int> UserIds { get; set; }
    }
}
