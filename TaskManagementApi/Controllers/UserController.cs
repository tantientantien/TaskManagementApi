using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;
using TaskManagementApi.Services;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Dtos;

namespace TaskManagementApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenService tokenService,
            ILogger<UserController> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
            _roleManager = roleManager;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(user => new UserDataDto
                {
                    Id = user.Id,
                    Username = user.UserName!,
                    Email = user.Email!
                })
                .ToListAsync();

            return Ok(new
            {
                status = "success",
                message = "Users retrieved successfully",
                data = users
            });
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid registration data", errors = ModelState });
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "Registration failed",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            var role = string.IsNullOrWhiteSpace(registerDto.Role) ? "User" : registerDto.Role;
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest(new { status = "error", message = $"Role '{role}' does not exist." });
            }

            await _userManager.AddToRoleAsync(user, role);

            return Ok(new
            {
                status = "success",
                message = "User registered successfully",
                data = new { user.UserName, user.Email, Role = role }
            });
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid login data", errors = ModelState });
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());
            if (user == null)
            {
                return Unauthorized(new { status = "error", message = "Invalid email or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { status = "error", message = "Invalid email or password" });
            }

            var token = await _tokenService.CreateToken(user, _userManager);

            Response.Cookies.Append("accessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new
            {
                status = "success",
                message = "Login successful",
                data = new { user.UserName, user.Email }
            });
        }

        // POST: api/users/logout
        [HttpPost("logout")]
        [Authorize]
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