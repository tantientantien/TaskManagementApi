using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Task;
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

        public TaskController(IGenericRepository<Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDataDto>>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAll();
            var taskDtos = tasks.Select(task => task.ToDataDto());

            return Ok(new { status = "success", message = "Tasks retrieved", data = taskDtos });
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
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
        public async Task<ActionResult<TaskDataDto>> AddTask([FromBody] TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid task data", errors = ModelState });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { status = "error", message = "Unauthorized: Missing userId" });
            }

            var task = taskDto.ToTask();
            task.UserId = userIdClaim;

            await _taskRepository.Add(task);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id },
                new { status = "success", message = "Task created", data = task.ToDataDto() });
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
            }

            var existingTask = await _taskRepository.GetById(id);
            if (existingTask == null)
            {
                return NotFound(new { status = "error", message = "Task not found" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || userIdClaim != existingTask.UserId)
            {
                return Forbid();
            }

            taskDto.UpdateTask(existingTask);
            await _taskRepository.Update(existingTask);

            return Ok(new { status = "success", message = "Task updated" });
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound(new { status = "error", message = "Task not found" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || userIdClaim != task.UserId)
            {
                return Forbid();
            }

            await _taskRepository.Delete(id);
            return NoContent();
        }
    }
}