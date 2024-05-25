namespace TaskManager.Models.Dto
{
    public class CreateChatDto
    {
        /// <summary>
        /// متن پیام
        /// </summary> 
        public string Text { get; set; }
        public int UserID { get; set; }
        public int ProjectId { get; set; }
    }
}
