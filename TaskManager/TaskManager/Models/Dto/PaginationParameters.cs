namespace TaskManager.Models.Dto;

public class PaginationParameters
{
    /// <example>
    /// 1
    /// </example>
    public int CurrentPage { get; set; }
    
    /// <example>
    /// 10
    /// </example>
    public int ItemsPerPage { get; set; }
}