using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TaskManager.Models;
using TaskManager.Models.Dto;
using TaskManager.Models.Entity;

namespace TaskManager.Controllers
{
    [Route("api/task")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly DataBaseContext _context;


        public TaskController(DataBaseContext context)
        {
            _context = context;
        }



        /// <summary>
        /// ایجاد یک وظیفه جدید
        /// </summary>
        /// <returns>پیام مناسب برای ایجاد شدن یا نشدن وظیفه.</returns>
        [HttpPost("create")]
        public IActionResult Create(CreateTaskDto task)
        {
            dynamic result = new JObject();
            try
            {

                var newTask = new Models.Entity.Task(task.Name, task.Description, task.ExpireTaskTime, 
                    task.PrioritySet,task.UserId,task.ProjectId);

                _context.Tasks.Add(newTask);
                _context.SaveChanges();

                result.message = "وظیفه با موفقیت ایجاد شد";
                result.success = true;

                return Ok(result);
            }
            catch
            {
                result.message = "وظیفه ایجاد نشد";
                return BadRequest(result);
            }
        }



       



        /// <summary>
        /// ویرایش وظیفه
        /// </summary>
        /// <returns>پیام مناسب برای ویرایش وظیفه یا خطا در صورت داشتن خطا</returns>
        [HttpPost("edit")]
        public IActionResult Edit([FromBody] EditTaskDto task)
        {
            dynamic result = new JObject();
          
                if (task == null)
                {
                    result.message = " وظیفه خود را وارد کنید!";
                    result.success = false;
                    return BadRequest(result);
                }

                var existingTask = _context.Tasks.Find(task.Id);
                if (existingTask == null)
                {
                    result.message = "وظیفه ای یافت نشد   !";
                    result.success = false;
                    return BadRequest(result);
                }
                // بررسی اینکه فیلدهای جدید خالی نباشند
                if (string.IsNullOrEmpty(task.Name) )
                {
                    result.message = "نام وظیفه نمی‌تواند خالی باشد!";
                    result.success = false;
                    return BadRequest(result);
                }

                existingTask.Name = task.Name;
                existingTask.Description = task.Description;
                existingTask.ExpireTaskTime = task.ExpireTaskTime;
                existingTask.PrioritySet = (Models.Entity.Task.Priority)task.PrioritySet;


                _context.SaveChanges();

                result.message = "وظیفه مورد نظر با موفقیت ویرایش شد";
                result.success = true;

                return Ok($"{existingTask} " +
                    $"{result}");
            
        
        }



        /// <summary>
        /// حذف وظیفه
        /// </summary>
        /// <returns>پیام مناسب برای حذف وظیفه یا خطا در صورت داشتن خطا</returns>
        [HttpPost("delete/{id}")]
        public IActionResult delete(int id)
        {
            dynamic result = new JObject();
            try
            {
                var existingTask = _context.Tasks.Find(id);
                if (existingTask == null)
                {
                    result.message = " وظیفه پیدا نشد ! ";
                    result.success = false;
                    return NotFound(result);
                }

                _context.Tasks.Remove(existingTask);
                _context.SaveChanges();

                result.message = "وظیفه با موفقیت حذف شد";
                result.success = true;

                return Ok(result);
            }
            catch
            {
                result.message = "وظیفه حذف نشد";
                return BadRequest(result);
            }
        }
        
        
        /// <summary>
        /// نمایش تمام وظیفه ها
        /// </summary>
        /// <returns>نمایش تمام وظیفه ها یا پیام مناسب در صورت خطا.</returns>
        [HttpGet("get-all")]
        public IActionResult GetTasks()
        {
            dynamic result = new JObject();

            try
            {
                var tasks = JArray.FromObject(_context.Tasks.Select(t => new TaskViewMode
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    ExpireTaskTime = t.ExpireTaskTime,
                    PrioritySet = t.PrioritySet,
                    UserId = t.UserId,
                    ProjectId = t.ProjectId,
                    // IsDone = t.IsDone
                }).ToList());

                result.alltasks = tasks;
                result.success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.message = "خطا در بازیابی وظایف: " + ex.Message;
                result.success = false;
                return BadRequest(result);
            }
        }
        
        
        /// <summary>
        /// نمایش وظیفه با ایدی وارد شده
        /// </summary>
        /// <returns>نمایش وظیفه با ایدی مشخص .</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            dynamic result = new JObject();

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                result.message = "وظیفه مورد نظر یافت نشد";
                result.success = false;
                return NotFound(result);
            }

            return Ok(task);
        }

    }
}
