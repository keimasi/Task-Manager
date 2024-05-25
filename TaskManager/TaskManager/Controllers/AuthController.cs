﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using TaskManager.Models;
using TaskManager.Models.Dto;
using TaskManager.Models.Entity;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly AuthService _authService;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        

        public AuthController(IConfiguration configuration
            , AuthService authService, DataBaseContext context)
        {
            _authService = authService;
            _context = context;
            _key = configuration["JwtConfig:Key"];
            _issuer = configuration["JwtConfig:Issuer"];
            _audience = configuration["JwtConfig:Audience"];
        }

        /// <summary>
        /// ورود کاربر به سیستم و صادر کردن توکن
        /// </summary>
        /// <param name="command">اطلاعات ورود نام کاربری و کلمه عبور.</param>
        /// <returns>اطلاعات ورود و توکن JWT در صورت موفقیت ورود یا پیام خطا در صورت وقوع مشکل.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDto command)
        {
            dynamic result = new JObject();

            if (!_authService.Authenticate(command))
            {
                result.message = "نام کاربری یا کلمه عبور اشتباه می باشد";
                result.success = false;
                return Unauthorized(result);
            }

            var user = _context.Users.FirstOrDefault(x => x.UserName == command.Username);

            if (user is { IsActive: false })
            {
                result.message = "شما دسترسی لازم را ندارید";
                result.success = false;
                return Ok(result);
            }

            // ایجاد Claims
            var claims = new List<Claim>
            {
                new("UserId", user.Id.ToString()),
                new("Name", command.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)); // کلید امضای توکن
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // الگوریتم امضا

            var tokenExp = DateTime.Now.AddMinutes(30);

            // تنظیمات توکن JWT
            var token = new JwtSecurityToken(
                issuer: _issuer, // صادرکننده
                audience: _audience, // دریافت‌کننده
                claims: claims, // لیست Claims
                expires: tokenExp, // زمان اعتبار توکن 30 دقیقه
                signingCredentials: creds // اطلاعات امضا
            );

            //ایجاد توکن نهایی برای کاربر
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            _context.UserTokens.Add(new UserToken
            {
                Token = jwtToken,
                TokenExp = tokenExp,
                User = user
            });

            _context.SaveChanges();

            result.message = "ورود با موفقیت انجام شد";
            result.success = true;
            result.accessToken = jwtToken;
            result.name = command.Username;

            return Ok(result);
        }


        /// <summary>
        /// دریافت اطلاعات کاربر بر اساس توکن.
        /// </summary>
        /// <param name="token">توکن احراز هویت کاربر.</param>
        /// <returns>
        /// اگر توکن معتبر باشد، اطلاعات کاربر را بازمی‌گرداند.
        /// در غیر این صورت، پیام خطا بازگردانده می‌شود.
        /// </returns>
        /// <response code="200">اگر توکن معتبر باشد، اطلاعات کاربر بازگردانده می‌شود.</response>
        /// <response code="404">اگر توکن معتبر نباشد، پیام خطا بازگردانده می‌شود.</response>
        [HttpGet("get-master")]
        public IActionResult GetMaster()
        {
            dynamic response = new JObject();
            
            var userDetails = _context.Users
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Avatar,
                    x.FirstName,
                    x.LastName,
                })
                .FirstOrDefault();
            
            return Ok(userDetails);
        }
        
        
        
        /// <summary>
        /// ایجاد یک کاربر جدید.
        /// </summary>
        /// <param name="model">اطلاعات مورد نیاز برای ایجاد کاربر.</param>
        /// <returns>پیام موفقیت آمیز یا پیام خطا در صورت وقوع مشکل.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> register([FromForm] RegisterUser model)
        {
            var image = "";
            string LastName = "";
            dynamic result = new JObject();

            // بررسی اینکه مدل خالی نباشد
            if (model == null)
            {
                result.message = "اطلاعات کاربر ارسال نشده است.";
                result.success = false;
                return BadRequest(result);
            }

            if (model.Password != model.PasswordConfirm)
            {
                result.message = "رمز عبور با یکدیگر مطابقت ندارند";
                result.success = false;
                return BadRequest(result);
            }

            // بررسی اینکه نام کاربری قبلاً ثبت نشده باشد
            if (await _context.Users.AnyAsync(x => x.UserName == model.UserName))
            {
                result.message = "نام کاربری وارد شده قبلا ثبت شده است.";
                result.success = false;
                return BadRequest(result);
            }
            // ایجاد کاربر جدید
            var user = new User(model.UserName, model.FirstName, LastName,model.Password,image);
            
            // اضافه کردن کاربر به دیتابیس
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var createdUser = await _context.Users.FirstOrDefaultAsync(x=> x.UserName==model.UserName);
            
            // ایجاد Claims
            var claims = new List<Claim>
            {
                new("UserId", createdUser.Id.ToString()),
                new("Name", createdUser.FirstName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)); // کلید امضای توکن
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // الگوریتم امضا

            var tokenExp = DateTime.Now.AddMinutes(30);

            // تنظیمات توکن JWT
            var token = new JwtSecurityToken(
                issuer: _issuer, // صادرکننده
                audience: _audience, // دریافت‌کننده
                claims: claims, // لیست Claims
                expires: tokenExp, // زمان اعتبار توکن 30 دقیقه
                signingCredentials: creds // اطلاعات امضا
            );
            
            //ایجاد توکن نهایی برای کاربر
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            _context.UserTokens.Add(new UserToken
            {
                Token = jwtToken,
                TokenExp = tokenExp,
                User = user
            });
            
                result.message = "کاربر با موفقیت ایجاد شد.";
                result.token = jwtToken;
                result.success = true;

                return Ok($"{result}");
        }
    }
}