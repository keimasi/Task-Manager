using static TaskManager.Models.Entity.Task;

namespace TaskManager.Models.Dto
{
    public class CreateTaskDto
    {
        /// <summary>
        /// نام وظیفه
        /// </summary> 
        public string Name { get; set; }
        /// <summary>
        ///توضیحات وظیفه
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        ///مهلت زمان وظیفه
        /// </summary> 
        public DateTime ExpireTaskTime { get; set; }
        /// <summary>
        ///سطح اهمیت وظیفه
        /// </summary> 
        public Priority PrioritySet { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
