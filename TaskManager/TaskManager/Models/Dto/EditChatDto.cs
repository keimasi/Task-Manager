namespace TaskManager.Models.Dto
{
    public class EditChatDto
    {

        public int Id { get; set; }
        /// <summary>
        ///متن پیام
        /// </summary> 
        public string Text { get; set; }
        /// <summary>
        ///تاریخ پیام ارسال شده
        /// </summary> 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
