using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly CookieService _cookieService;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenService tokenService,
            RoleManager<IdentityRole<int>> roleManager,
            IMapper mapper,
            CookieService cookieService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _mapper = mapper;
            _cookieService = cookieService;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Get all users", Description = "Requires Admin role")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDataDto>>(users);
            return Ok(new { status = "success", message = "Users retrieved successfully", data = userDtos });
        }



        // POST: api/users/register
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user", Description = "Creates a new user with an optional role (default is 'User')")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "error", message = "Invalid registration data", errors = ModelState });

            var user = new User { UserName = registerDto.Username, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new { status = "error", message = "Registration failed", errors = result.Errors.Select(e => e.Description) });

            var role = string.IsNullOrWhiteSpace(registerDto.Role) ? "User" : registerDto.Role;
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.DeleteAsync(user); // rollback
                return BadRequest(new { status = "error", message = $"Role '{role}' does not exist" });
            }

            await _userManager.AddToRoleAsync(user, role);
            var userDto = _mapper.Map<UserDataDto>(user);

            return Ok(new { status = "success", message = "User registered successfully", data = userDto, role });
        }

        // POST: api/users/login
        [HttpPost("login")]
        [SwaggerOperation(Summary = "User login", Description = "Authenticates a user and sets a JWT token in an HTTP-only cookie")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(loginDto.Email))
                return BadRequest(new { status = "error", message = "Invalid login data or email required", errors = ModelState });

            var user = await _userManager.FindByEmailAsync(loginDto.Email.ToLower());
            if (user == null)
                return Unauthorized(new { status = "error", message = "Invalid email or password" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { status = "error", message = "Invalid email or password" });

            var token = await _tokenService.CreateToken(user, _userManager);
            _cookieService.SetCookie("accessToken", token);

            return Ok(new { status = "success", message = "Login successful", data = _mapper.Map<UserDataDto>(user) });
        }


        // POST: api/users/logout
        [HttpPost("logout")]
        [SwaggerOperation(Summary = "User logout", Description = "Deletes the authentication token from cookies")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _cookieService.RemoveCookie("accessToken");

            return Ok(new { status = "success", message = "Logout successful" });
        }

    }
}