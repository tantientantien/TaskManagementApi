using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Task;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IGenericRepository<Task> _taskRepository;
        private readonly UserManager<User> _userManager;

        public TaskController(IGenericRepository<Task> taskRepository, UserManager<User> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        // GET: api/tasks
        [HttpGet]
        [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieves all tasks available in the system")]
        public async Task<ActionResult<IEnumerable<TaskDataDto>>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAll();
            var taskDtos = tasks.Select(task => task.ToDataDto());

            return Ok(new { status = "success", message = "Tasks retrieved", data = taskDtos });
        }

        // GET: api/tasks/{id}
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get task by ID", Description = "Retrieves a task using its unique ID")]
        public async Task<ActionResult<TaskDataDto>> GetTaskById(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound(new { status = "error", message = "Task not found" });
            }

            return Ok(new { status = "success", message = "Task found", data = task.ToDataDto() });
        }

        // POST: api/tasks
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create a new task", Description = "Requires authentication. Adds a new task to the system")]
        public async Task<ActionResult<TaskDataDto>> AddTask([FromBody] TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid task data", errors = ModelState });
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { status = "error", message = "Unauthorized: Missing userId" });
            }

            var task = taskDto.ToTask();
            task.UserId = userId;

            await _taskRepository.Add(task);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id },
                new { status = "success", message = "Task created", data = task.ToDataDto() });
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "User")]
        [SwaggerOperation(Summary = "Update a task", Description = "Only the task owner can update their task. Requires authentication")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
            }

            var storedTask = await _taskRepository.GetById(id);
            if (storedTask == null)
            {
                return NotFound(new { status = "error", message = "Task not found" });
            }

            var userIdClaim = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdClaim) || userIdClaim != storedTask.UserId)
            {
                return Forbid();
            }

            taskDto.UpdateTask(storedTask);
            await _taskRepository.Update(storedTask);

            return Ok(new { status = "success", message = "Task updated" });
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        [SwaggerOperation(Summary = "Delete a task", Description = "Only the task owner or an admin can delete a task. Requires authentication")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound(new { status = "error", message = "Task not found" });
            }

            var userIdClaim = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdClaim) || userIdClaim != task.UserId)
            {
                return Forbid();
            }

            await _taskRepository.Delete(id);
            return Ok(new { status = "success", message = "Task deleted" });
        }
    }
}