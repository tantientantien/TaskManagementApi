using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        //GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDataDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();

            var data = users.Select(c => new UserDataDto
            {
                Id = c.Id,
                Username = c.Username,
                Email = c.Email,
            });
            return Ok(new { status = "success", message = "Get all users successfully", data = data });
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _userService.RegisterUser(registerDto);
            if (!result.Success)
            {
                return Conflict(new { status = "error", message = result.Message });
            }

            return CreatedAtAction(nameof(Register), new { id = result.User!.Id },
                new { status = "success", message = result.Message, data = new { result.User.Username, result.User.Email } });
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _userService.AuthenticateUser(loginDto);
            if (!result.Success)
            {
                return Unauthorized(new { status = "error", message = result.Message });
            }

            Response.Cookies.Append("accessToken", result.Token!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new { status = "success", message = result.Message, data = new { result.User!.Username, result.User.Email } });
        }

        // POST: api/users/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            });

            return Ok(new { status = "success", message = "Logout successful" });
        }
    }
}