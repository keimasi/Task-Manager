using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TaskManager.Models;
using TaskManager.Models.Dto;
using TaskManager.Models.Entity;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/project")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly DataBaseContext _context;
        

        public ProjectController(DataBaseContext context)
        {
            _context = context;
        }

        

        /// <summary>
        /// ویرایش پروژه
        /// </summary>
        /// <returns>پیام مناسب برای ویرایش شدن یا نشدن پروژه.</returns>
        [HttpPost("edit")]
        public IActionResult Edit([FromBody] EditProjectDto project)
        {
            dynamic result = new JObject();
            try
            {
                if (project == null)
                {
                    result.message = "پروژه ای وارد نکردید!";
                    result.success = false;
                    return BadRequest(result);
                }

                var existingProject = _context.Projects.Find(project.Id);
                if (existingProject == null)
                {
                    result.message = "پروژه یافت نشد !";
                    result.success = false;
                    return BadRequest(result);
                }
                // بررسی اینکه فیلدهای جدید خالی نباشند
                if (string.IsNullOrEmpty(project.Name) )
                {
                    result.message = "نام پروژه نمی‌تواند خالی باشد!";
                    result.success = false;
                    return BadRequest(result);
                }

                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.StartTime = project.StartTime;
                existingProject.EndTime = project.EndTime;


                _context.SaveChanges();

                result.message = "پروژه مورد نظر با موفقیت ویرایش شد";
                result.success = true;

                return Ok($"{existingProject} " +
                    $"{result}");
            }
            catch
            {
                return BadRequest("خطایی رخ داده است!");
            }
        }




        /// <summary>
        /// فعال یا غیرفعال کردن یک پروژه.
        /// </summary>
        /// <param name="id">شناسه پروژه مورد نظر.</param>
        /// <returns>پیام موفقیت آمیز یا پیام خطا در صورت وقوع مشکل.</returns>
        [HttpPost("toggle-active/{id}")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            dynamic result = new JObject();

            var usprojecter = await _context.Projects.FindAsync(id);
            if (usprojecter == null)
            {
                result.message = "پروزه ای یافت نشد !";
                result.success = false;
                return NotFound(result);
            }

            usprojecter.IsActive = !usprojecter.IsActive;

            await _context.SaveChangesAsync();

            result.message = usprojecter.IsActive ? "پروژه فعال شد" : "پروژه غیرفعال شد";
            result.success = true;

            return Ok(result);
        }



        /// <summary>
        /// نمایش همه پروژه ها
        /// </summary>
        /// <returns>نمایش همه پروژه ها .</returns>
        [HttpGet("get-all")]
        public IActionResult GetProjects()
        {
            dynamic result = new JObject();

            try
            {
                var projects = JArray.FromObject(_context.Projects.Select(t => new GetAllProjectsDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    EndTime = t.EndTime,
                    StartTime = t.StartTime,
                    IsActive = t.IsActive,
                    UserProjects = t.UserProjects
                    
                }).ToList());

                result.Projects = projects;
                result.success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.message = "خطا در بازیابی پروژه: " + ex.Message;
                result.success = false;
                return BadRequest(result);
            }
        }
        
        /// <summary>
        /// نمایش پروژه با ایدی وارد شده
        /// </summary>
        /// <returns>نمایش پروژه با ایدی مشخص .</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            dynamic result = new JObject();

            var project = await _context.Projects.FindAsync(id);
            
            if (project == null)
            {
                result.message = "پروژه مورد نظر یافت نشد";
                result.success = false;
                return NotFound(result);
            }
            
            return Ok(project);
        }
        
        
        
        /// <summary>
        /// ایجاد یک پروژه جدید
        /// </summary>
        /// <returns>پیام مناسب برای ایجاد شدن یا نشدن پروژه.</returns>
        // [HttpPost("create")]
        // public IActionResult Create([FromBody] CreateProjectDto project)
        // {
        //     dynamic result = new JObject();
        //
        //     if (project == null)
        //     {
        //         result.message = "اطلاعات پروژه نادرست است";
        //         result.success = false;
        //         return BadRequest(result);
        //     }
        //
        //     var newProject = new Project(
        //         
        //         project.Name,
        //         project.Description,
        //         project.StartTime,
        //         project.EndTime
        //         
        //     );
        //     _context.Projects.Add(newProject);
        //     _context.SaveChanges();
        //
        //     // Add users to the project
        //     if (project.UserIds != null && project.UserIds.Any())
        //     {
        //         foreach (var userId in project.UserIds)
        //         {
        //             var user = _context.Users.Find(userId);
        //             
        //             if (user != null)
        //             {
        //                 newProject.UserProjects.Add(new UserProject { UserId = userId });
        //                 
        //             }
        //         }
        //     }
        //     _context.SaveChanges();
        //     
        //     result.message = "پروژه با موفقیت ایجاد شد";
        //     result.success = true;
        //
        //     return Ok(result);
        // }
        
        
        
        /// <summary>
        /// ایجاد یک پروژه جدید
        /// </summary>
        /// <returns>پیام مناسب برای ایجاد شدن یا نشدن پروژه.</returns>
        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateProjectDto project)
        {
            dynamic result = new JObject();

            if (project == null)
            {
                result.message = "اطلاعات پروژه نادرست است";
                result.success = false;
                return BadRequest(result);
            }

            // ایجاد پروژه جدید
            var newProject = new Project(
                project.Name,
                project.Description,
                project.StartTime,
                project.EndTime
            );
    
            // اضافه کردن پروژه به دیتابیس
            _context.Projects.Add(newProject);
            _context.SaveChanges();

            // اضافه کردن کاربران به پروژه
            if (project.UserIds != null && project.UserIds.Any())
            {
                foreach (var userId in project.UserIds)
                {
                    var user = _context.Users.Find(userId);
                    if (user != null)
                    {
                        _context.UserProjects.Add(new UserProject { UserId = userId, ProjectId = newProject.Id });
                    }
                }
                _context.SaveChanges();
            }
    
            result.message = "پروژه با موفقیت ایجاد شد";
            result.success = true;

            return Ok(result);
        }
        
        
        
        
        /// <summary>
        /// نمایش تمام پروژه های مربوط به یک کاربر.
        /// </summary>
        /// <param name="userId">شناسه کاربر.</param>
        /// <returns>نمایش پروژه کاربر یا پیام مناسب در صورت خطا.</returns>
        [HttpGet("get-all/{userId}")]
        public IActionResult GetProjectByUserId(int userId)
        {
            dynamic result = new JObject();
            try
            {
                // فیلتر کردن پروژه ها بر اساس UserId
                var projectIds = _context.UserProjects
                    .Where(up => up.UserId == userId)
                    .Select(up => up.ProjectId)
                    .ToList();

                if (projectIds == null || !projectIds.Any())
                {
                    result.message = "هیچ پروژه‌ای یافت نشد.";
                    result.success = false;
                    return NotFound(result);
                }

                // دریافت پروژه ها بر اساس ProjectId
                var projects = _context.Projects
                    .Where(p => projectIds.Contains(p.Id))
                    .Select(t => new GetAllProjectsDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
                        IsActive = t.IsActive,
                    })
                    .ToList();

                result.allprojects = JArray.FromObject(projects);
                result.success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.message = "خطا در بازیابی پروژه‌ها: " + ex.Message;
                result.success = false;
                return BadRequest(result);
            }
        }
    }
}
