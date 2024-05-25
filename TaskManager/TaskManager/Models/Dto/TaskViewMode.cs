using static TaskManager.Models.Entity.Task;

namespace TaskManager.Models.Dto
{
    public class TaskViewMode
    {
        /// <summary>
        ///ایدی وظیفه
        public int Id { get; set; }
        /// <summary>
        ///نام وظیفه
        /// </summary> 
        public string Name { get; set; }
        /// <summary>
        ///توضیحات وظیفه
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        ///مهلت وظیفه
        /// </summary> 
        public DateTime ExpireTaskTime { get; set; }
        /// <summary>
        ///سطح اهمیت وظیفه
        /// </summary>
        public Priority PrioritySet { get; set; }
        ///وظیفه انجام شده یا نه
        /// </summary>
        // public bool IsDone { get; set; }
        
        
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
