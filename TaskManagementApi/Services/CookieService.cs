using Microsoft.AspNetCore.Http;
using System;

namespace TaskManagementApi.Services
{
    public class CookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int days = 7, bool httpOnly = true, bool secure = true, SameSiteMode sameSite = SameSiteMode.Strict)
        {
            var options = new CookieOptions
            {
                HttpOnly = httpOnly,
                Secure = secure,
                SameSite = sameSite,
                Expires = DateTimeOffset.UtcNow.AddDays(days)
            };
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
        }

        public string? GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[key];
        }

        public void RemoveCookie(string key)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }
    }
}