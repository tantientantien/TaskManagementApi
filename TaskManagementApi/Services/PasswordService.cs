using System;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagementApi.Services
{
    public static class PasswordService
    {
        public static string GenerateSalt(int size = 32)
        {
            var saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt, string pepper)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(pepper)))
            {
                var combinedPassword = Encoding.UTF8.GetBytes(password + salt);
                var hashBytes = hmac.ComputeHash(combinedPassword);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}