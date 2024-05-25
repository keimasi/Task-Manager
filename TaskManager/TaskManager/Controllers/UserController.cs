using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TaskManager.Models;
using TaskManager.Models.Dto;
using TaskManager.Models.Entity;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly FileUpload _fileUpload;

        public UserController(FileUpload fileUpload, DataBaseContext context)
        {
            _fileUpload = fileUpload;
            _context = context;
        }

        /// <summary>
        /// دریافت لیستی از کاربران با صفحه‌بندی.
        /// </summary>
        /// <returns>لیست کاربران به همراه اطلاعات صفحه‌بندی.</returns>
        [HttpPost("get-all")]
        public async Task<IActionResult> GetAll([FromBody] PaginationParameters paginationParams)
        {
            try
            {
                int skip = (paginationParams.CurrentPage - 1) * paginationParams.ItemsPerPage;

                var usersQuery = _context.Users
                    .Select(x => new UserViewModel
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Password = x.Password,
                        Avatar = x.Avatar,
                        IsActive = x.IsActive
                    });

                var users = await usersQuery
                    .Skip(skip)
                    .Take(paginationParams.ItemsPerPage)
                    .ToListAsync();

                if (users.Count == 0)
                {
                    return NotFound("کاربری یافت نشد!");
                }

                var totalCount = await usersQuery.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.ItemsPerPage);

                var paginationHeader = new
                {
                    paginationParams.CurrentPage,
                    paginationParams.ItemsPerPage,
                    totalCount,
                    totalPages
                };

                return Ok(new { users, paginationHeader });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "خطایی رخ داده است!");
            }
        }


        /// <summary>
        /// دریافت اطلاعات یک کاربر بر اساس شناسه.
        /// </summary>
        /// <param name="id">شناسه کاربر مورد نظر.</param>
        /// <returns>اطلاعات کاربر یا پیام خطا در صورت عدم یافتن.</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            dynamic result = new JObject();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                result.message = "کاربر مورد نظر یافت نشد";
                result.success = false;
                return NotFound(result);
            }

            return Ok(user);
        }


        /// <summary>
        /// ایجاد یک کاربر جدید.
        /// </summary>
        /// <param name="model">اطلاعات مورد نیاز برای ایجاد کاربر.</param>
        /// <returns>پیام موفقیت آمیز یا پیام خطا در صورت وقوع مشکل.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateUserDto model)
        {
            dynamic result = new JObject();

            if (await _context.Users.AnyAsync(x => x.UserName == model.UserName))
            {
                result.message = "نام کاربری وارد شده قبلا ثبت شده است .";
                result.success = false;
                return BadRequest(result);
            }

            var userAvatar = await _fileUpload.Upload(model.Avatar);
            var user = new User(model.UserName, model.FirstName, model.LastName, model.Password, userAvatar);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            result.message = "کاربر با موفقیت ایجاد شد";
            result.success = true;

            return Ok(result);
        }


        /// <summary>
        /// به‌روزرسانی اطلاعات یک کاربر.
        /// </summary>
        /// <param name="model">اطلاعات جدید کاربر برای به‌روزرسانی.</param>
        /// <returns>پیام موفقیت آمیز یا پیام خطا در صورت وقوع مشکل.</returns>
        [HttpPost("edit")]
        public async Task<IActionResult> UpdateAsync([FromForm] EditUserDto model)
        {
            dynamic result = new JObject();

            var existingUser = await _context.Users.FindAsync(model.Id);
            if (existingUser == null)
            {
                result.message = "کاربری یافت نشد !";
                result.success = false;
                return BadRequest(result);
            }

            existingUser.FirstName = model.FirstName ?? existingUser.FirstName;
            existingUser.LastName = model.LastName ?? existingUser.LastName;

            if (model.Avatar != null)
            {
                var newFileName = await _fileUpload.Upload(model.Avatar);

                if (!string.IsNullOrEmpty(newFileName))
                {
                    if (!string.IsNullOrEmpty(existingUser.Avatar))
                    {
                        _fileUpload.Delete(existingUser.Avatar);
                    }

                    existingUser.Avatar = newFileName;
                }
                else
                {
                    result.message = "آپلود تصویر ناموفق بود !";
                    result.success = false;
                    return BadRequest(result);
                }
            }

            await _context.SaveChangesAsync();

            result.message = "کاربر مورد نظر با موفقیت ویرایش شد";
            result.success = true;

            return Ok(result);
        }


        /// <summary>
        /// تغییر رمز عبور کاربر.
        /// </summary>
        /// <param name="model.Id"></param>
        /// <param name="model.OldPassword"></param>
        /// <param name="model.NewPassword"></param>
        /// <returns>پیام موفقیت آمیز یا پیام خطا در صورت وقوع مشکل.</returns>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePassword model)
        {
            dynamic result = new JObject();

            var user = await _context.Users.FindAsync(model.Id);

            if (user == null)
            {
                result.message = "کاربری یافت نشد !";
                result.success = false;
                return BadRequest(result);
            }

            if (model.OldPassword != user.Password)
            {
                result.message = "رمز عبور وارد شده با رمز عبور قبلی مطابقت ندارد !";
                result.success = false;
                return BadRequest(result);
            }

            user.ChangePassword(model.NewPassword);

            await _context.SaveChangesAsync();

            result.message = "پسورد کاربر مورد نظر با موفقیت ویرایش شد !";
            result.success = true;

            return Ok(result);
        }


        /// <summary>
        /// فعال یا غیرفعال کردن یک کاربر.
        /// </summary>
        /// <param name="id">شناسه کاربر مورد نظر.</param>
        /// <returns>پیام موفقیت آمیز یا پیام خطا در صورت وقوع مشکل.</returns>
        [HttpPost("toggle-active/{id}")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            dynamic result = new JObject();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                result.message = "کاربری یافت نشد !";
                result.success = false;
                return NotFound(result);
            }

            user.IsActive = !user.IsActive;

            await _context.SaveChangesAsync();

            result.message = user.IsActive ? "کاربر فعال شد" : "کاربر غیرفعال شد";
            result.success = true;

            return Ok(result);
        }
    }
}