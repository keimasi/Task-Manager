using TaskManager.Models.Entity;

namespace TaskManager.Models.Dto;

public class CommentViewModel
{
    /// <summary>
    /// عنوان نظر
    /// </summary> 
    public string Title { get; set; }
    /// <summary>
    /// متن نظر
    /// </summary> 
    public string Text { get; set; }
    /// <summary>
    ///   تاریخ ساخته شدن نظر
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
    public User User { get; set; }
    public Project Project { get; set; }
}