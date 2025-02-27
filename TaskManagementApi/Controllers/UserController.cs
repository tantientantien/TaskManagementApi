using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;
using TaskManagementApi.Services;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Dtos;
using TaskManagementApi.Mappers;

namespace TaskManagementApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenService tokenService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Get all users", Description = "Requires Admin role")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(user => UserMapper.ToDataDto(user))
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
        [SwaggerOperation(Summary = "Register a new user", Description = "Creates a new user with an optional role (default is 'User'), or the user role can be set to 'Admin'")]
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
                await _userManager.DeleteAsync(user);
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
        [SwaggerOperation(Summary = "User login", Description = "Authenticates a user and sets a JWT token in an HTTP-only cookie")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid login data", errors = ModelState });
            }

            var email = loginDto.Email?.ToLower();
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { status = "error", message = "Email is required" });
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
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
        [SwaggerOperation(Summary = "User logout", Description = "Deletes the authentication token from cookies")]
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