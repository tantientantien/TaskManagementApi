using TaskManagementApi.Dtos;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message, User? User)> RegisterUser(RegisterDto registerDto);
        Task<(bool Success, string Message, string? Token, User? User)> AuthenticateUser(LoginDto loginDto);
    }
}