using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        public IActionResult GetMaster(string token)
        {
            dynamic response = new JObject();

            var userTokenRecord = _context.UserTokens
                .AsNoTracking()
                .FirstOrDefault(x => x.Token == token);

            if (userTokenRecord == null || userTokenRecord.TokenExp < DateTime.Now)
            {
                response.message = "توکن معتبر نمی باشد !";
                response.success = false;
                return NotFound(response);
            }

            var userDetails = _context.Users
                .AsNoTracking()
                .Where(x => x.Id == userTokenRecord.UserId)
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
    }
}