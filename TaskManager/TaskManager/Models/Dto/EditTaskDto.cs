using TaskManager.Models.Entity;

namespace TaskManager.Models.Dto
{
    public class EditTaskDto
    {
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
        ///مهلت زمان وظیفه
        /// </summary> 
        public DateTime ExpireTaskTime { get; set; }
        /// <summary>
        ///سطح اهمیت وظیفه
        /// </summary> 
        public EnumTaskPriority PrioritySet { get; set; }
    }
}
