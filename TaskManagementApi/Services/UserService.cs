using System.Net;
using TaskManagementApi.Dtos;
using TaskManagementApi.Models;
using TaskManagementApi.Repository;

namespace TaskManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly TokenService _tokenService;

        public UserService(IUserRepository userRepository, IConfiguration config, TokenService tokenService)
        {
            _userRepository = userRepository;
            _config = config;
            _tokenService = tokenService;
        }

        public async Task<(bool Success, string Message, User? User)> RegisterUser(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.FindByEmail(registerDto.Email);
            if (existingUser != null)
            {
                return (false, "Email is already taken.", null);
            }
            var salt = PasswordService.GenerateSalt();
            var pepper = _config["Security:Pepper"];
            var passwordHash = PasswordService.HashPassword(registerDto.Password, salt, pepper);

            var newUser = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Salt = salt
            };

            await _userRepository.Add(newUser);
            return (true, "Register successful", newUser);
        }

        public async Task<(bool Success, string Message, string? Token, User? User)> AuthenticateUser(LoginDto loginDto)
        {
            var user = await _userRepository.FindByEmail(loginDto.Email);
            if (user == null)
            {
                return (false, "Invalid email or password.", null, null);
            }

            var pepper = _config["Security:Pepper"];
            var hashedPassword = PasswordService.HashPassword(loginDto.Password, user.Salt, pepper);
            if (hashedPassword != user.PasswordHash)
            {
                return (false, "Invalid email or password.", null, null);
            }

            var token = _tokenService.CreateToken(user);
            return (true, "Login successful", token, user);
        }
    }
}