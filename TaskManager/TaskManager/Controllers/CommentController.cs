using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TaskManager.Models;
using TaskManager.Models.Dto;
using TaskManager.Models.Entity;

namespace TaskManager.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly DataBaseContext _context;
        
        public CommentController(DataBaseContext context)
        {
            _context = context;
        }



        /// <summary>
        /// ایجاد یک نظر جدید
        /// </summary>
        /// <returns>پیام مناسب برای ایجاد شدن یا نشدن نظر.</returns>
        [HttpPost("create")]
        public IActionResult Create([FromForm] CreateCommentDto comment)
        {
            dynamic result = new JObject();

            try
            {
                if (comment == null)
                {
                    result.message = " نظر خود را واررد کنید";
                    result.success = false;
                    return BadRequest(result);
                }

                var newComment = new Comment(comment.Title, comment.Text,comment.UserID,comment.TaskId);

                _context.Comments.Add(newComment);
                _context.SaveChanges();

                result.message = "نظر شما با موفقیت ایجاد شد";
                result.success = true;

                return Ok(result);
            }
            catch 
            {
                result.message = "نظر ایجاد نشد";
                result.success = false;

                return BadRequest(result);
            }
            
        }



        /// <summary>
        /// حذف یک نظر 
        /// </summary>
        /// <returns>پیام مناسب برای حذف شدن یا نشدن نظر.</returns>
        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            dynamic result = new JObject();
            try
            {
                var comment = _context.Comments.Find(id);

                if (comment == null)
                {
                    result.message = "نظر یافت نشد!";
                    result.success = false;
                    return NotFound(result);
                }

                _context.Comments.Remove(comment);
                _context.SaveChanges();

                result.message = "نظر با موفقیت حذف شد";
                result.success = true;

                return Ok(result);
            }
            catch
            {
                result.message = "نظر حذف نشد";
                result.success = false;
                return BadRequest(result);
            }

        }
    


    /// <summary>
        /// ویرایش یک نظر 
        /// </summary>
        /// <returns>پیام مناسب برای ویرایش شدن یا نشدن نظر.</returns>
        [HttpPost("edit")]
        public IActionResult Edit([FromBody] EditCommentDto comment)
        {
            dynamic result = new JObject();
            try
            {
                if (comment == null)
                {
                    result.message = " نظر خود را وارد کنید!";
                    result.success = false;
                    return BadRequest(result);
                }

                var existingComment = _context.Comments.Find(comment.Id);
                if (existingComment == null)
                {
                    result.message = "نظری یافت نشد   !";
                    result.success = false;
                    return BadRequest(result);
                }
                // بررسی اینکه فیلدهای جدید خالی نباشند
                if (string.IsNullOrEmpty(comment.Title) || string.IsNullOrEmpty(comment.Text))
                {
                    result.message = "عنوان و متن نظر نمی‌توانند خالی باشند!";
                    result.success = false;
                    return BadRequest(result);
                }

                existingComment.Title = comment.Title;
                existingComment.Text = comment.Text;


                _context.SaveChanges();

                result.message = "نظر مورد نظر با موفقیت ویرایش شد";
                result.success = true;

                return Ok($"{existingComment} " +
                          $"{result}");
            }
            catch 
            {
                result.message = "نظر مورد نظر ویرایش نشد";
                result.success = false;
                return BadRequest(result);
            }
        }
    
    
        /// <summary>
        /// دریافت نظرات براساس TaskId
        /// </summary>
        /// <param name="taskId">شناسه وظیفه</param>
        /// <returns>لیست نظرات مربوط به وظیفه یا پیام مناسب در صورت خطا</returns>
        [HttpGet("comments/by-task/{taskId}")]
        public IActionResult GetCommentsByTaskId(int taskId)
        {
            dynamic result = new JObject();

            try
            {
                var comments = _context.Comments
                    .Where(c => c.TaskId == taskId)
                    .Select(c => new
                    {
                        c.Id,
                        c.Title,
                        c.Text,
                        c.CreatedAt,
                        User = new 
                        {
                            c.User.Id,
                            c.User.UserName
                        }
                    })
                    .ToList();

                if (comments.Count == 0)
                {
                    result.message = "هیچ نظری یافت نشد";
                    result.success = false;
                    return NotFound(result);
                }

                result.comments = JArray.FromObject(comments);
                result.success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.message = "خطا در بازیابی نظرات: " + ex.Message;
                result.success = false;
                return BadRequest(result);
            }
        }
        
        /// <summary>
        /// دریافت نظرات براساس UserId
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>لیست نظرات مربوط به کاربر یا پیام مناسب در صورت خطا</returns>
        [HttpGet("comments/by-user/{userId}")]
        public IActionResult GetCommentsByUserId(int userId)
        {
            dynamic result = new JObject();

            try
            {
                var comments = _context.Comments
                    .Where(c => c.UserId == userId)
                    .Select(c => new
                    {
                        c.Id,
                        c.Title,
                        c.Text,
                        c.CreatedAt,
                        Task = new 
                        {
                            c.Task.Id,
                            c.Task.Name
                        }
                    })
                    .ToList();

                if (comments.Count == 0)
                {
                    result.message = "هیچ نظری یافت نشد";
                    result.success = false;
                    return NotFound(result);
                }

                result.comments = JArray.FromObject(comments);
                result.success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.message = "خطا در بازیابی نظرات: " + ex.Message;
                result.success = false;
                return BadRequest(result);
            }
        }
        
    }
}
