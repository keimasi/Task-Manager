using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TaskManager.Models;
using TaskManager.Models.Dto;
using TaskManager.Models.Entity;

namespace TaskManager.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly DataBaseContext _context;



        public ChatController(DataBaseContext context)
        {
            _context = context;
        }
        
        
        /// <summary>
        /// ایجاد یک پیام جدید
        /// </summary>
        /// <returns>پیام مناسب برای ایجاد شدن یا نشدن پیام.</returns>
        [HttpPost("create")]
        public IActionResult create([FromForm] CreateChatDto chat)
        {
            dynamic result = new JObject();
        
            try
            {
                if (chat == null)
                {
                    result.message = "لطفا فیلد هارا بدرستی وارد کنید";
                    result.success = false;
                    return BadRequest(result);
                }
        
                var newComment = new Chat(chat.Text,chat.UserID,chat.ProjectId);
        
                _context.Chats.Add(newComment);
                _context.SaveChanges();
        
                result.message = "پیام شما با موفقیت ارسال شد";
                result.success = true;
        
                return Ok(result);
            }
            catch (Exception e)
            {
                result.message = "پیام شماارسال نشد";
                result.success = false;
        
                return BadRequest($"{result} + {e}");
            }
           
        }
        
        
        /// <summary>
        /// حذف  پیام 
        /// </summary>
        /// <returns>پیام مناسب برای حذف شدن یا نشدن پیام.</returns>
        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            dynamic result = new JObject();
        
            try
            {
                var chat = _context.Chats.Find(id);
        
                if (chat == null)
                {
                    result.message = "پیام یافت نشد!";
                    result.success = false;
                    return NotFound(result);
                }
        
                _context.Chats.Remove(chat);
                _context.SaveChanges();
        
                result.message = "پیام با موفقیت حذف شد";
                result.success = true;
        
                return Ok(result);
            }
            catch 
            {
                result.message = "پیام حذف نشد";
                result.success = false;
        
                return BadRequest(result);
            }
        }
        
        
        
        
        /// <summary>
        /// ویرایش  پیام 
        /// </summary>
        /// <returns>پیام مناسب برای ویرایش شدن یا نشدن پیام.</returns>
        [HttpPost("edit")]
        public IActionResult Edit([FromBody] EditChatDto chat)
        {
            dynamic result = new JObject();
        
            try
            {
                if (chat == null)
                {
                    result.message = " پیام خود را وارد کنید!";
                    result.success = false;
                    return BadRequest(result);
                }
                var existingChat = _context.Chats.Find(chat.Id);
                if (existingChat == null)
                {
                    result.message = "پیامی یافت نشد   !";
                    result.success = false;
                    return BadRequest(result);
                }
                existingChat.Text = chat.Text;
                _context.SaveChanges();
        
                result.message = "پیام مورد نظر با موفقیت ویرایش شد";
                result.success = true;
        
                return Ok($"{existingChat} " +
                          $"{result}");
            }
            catch 
            {
                result.message = "پیام مورد نظر ویرایش نشد";
                result.success = false;
        
                return BadRequest(result);
            }
        }
    }
}
