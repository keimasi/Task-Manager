namespace TaskManager.Models.Dto;

public class ChatViewModel
{
    /// <summary>
    /// متن پیام
    /// </summary> 
    public string Text { get; set; }
    /// <summary>
    /// تاریخ ساخته شدن پیام
    /// </summary> 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    /// <summary>
    /// ایدی کاربر
    /// </summary> 
    public int UserID { get; set; }
    /// <summary>
    /// ایدی پروژه
    /// </summary> 
    public int ProjectId { get; set; }
}