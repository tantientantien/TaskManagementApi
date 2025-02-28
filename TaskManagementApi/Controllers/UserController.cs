using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IMapper _mapper;

        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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

        // POST: api/users/roles
        [HttpPost("roles")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Create a new role", Description = "Requires Admin role")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest(new { status = "error", message = "Role name is required." });

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return BadRequest(new { status = "error", message = "Role already exists." });

            var role = new IdentityRole<int> { Name = roleName, NormalizedName = roleName.ToUpper() };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return BadRequest(new { status = "error", message = "Failed to create role.", errors = result.Errors });

            return Ok(new { status = "success", message = "Role created successfully." });
        }

        // POST: api/users/{userId}/roles
        [HttpPost("{userId}/roles")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Assign a role to a user", Description = "Requires Admin role")]
        public async Task<IActionResult> AssignRole(int userId, [FromBody] string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound(new { status = "error", message = "User not found." });

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return BadRequest(new { status = "error", message = "Role does not exist." });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                return BadRequest(new { status = "error", message = "Failed to assign role.", errors = result.Errors });

            return Ok(new { status = "success", message = "Role assigned successfully." });
        }
    }
}
