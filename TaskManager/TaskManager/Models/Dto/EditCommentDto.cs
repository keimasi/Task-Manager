namespace TaskManager.Models.Dto
{
    public class EditCommentDto
    {
        public int Id { get; set; }
        /// <summary>
        ///عنوان نظر
        /// </summary> 
        public string Title { get; set; }
        /// <summary>
        ///متن نظر
        /// </summary> 
        public string Text { get; set; }
        /// <summary>
        ///زمان ایجاد نظر
        /// </summary> 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
