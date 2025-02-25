using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Task;
using TaskManagementApi.Mappers;
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
            var taskDtos = tasks.Select(TaskMapper.MapToDataDto);

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

            var taskDto = TaskMapper.MapToDataDto(task);
            return Ok(new { status = "success", message = "Task found", data = taskDto });
        }

        // POST: api/tasks/{categoryId}
        [Authorize]
        [HttpPost]
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

            var task = TaskMapper.MapFromCreateDto(taskDto);
            task.UserId = int.Parse(userIdClaim);

            await _taskRepository.Add(task);
            var taskDtoResponse = TaskMapper.MapToDataDto(task);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id },
                new { status = "success", message = "Task created", data = taskDtoResponse });
        }

        // PUT: api/tasks/{id}
        [Authorize]
        [HttpPut("{id}")]
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
            if (string.IsNullOrEmpty(userIdClaim) || int.Parse(userIdClaim) != existingTask.UserId)
            {
                return Forbid();
            }

            TaskMapper.MapFromUpdateDto(taskDto, existingTask);
            await _taskRepository.Update(existingTask);

            return Ok(new { status = "success", message = "Task updated" });
        }

        // DELETE: api/tasks/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound(new { status = "error", message = "Task not found" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || int.Parse(userIdClaim) != task.UserId)
            {
                return Forbid();
            }

            await _taskRepository.Delete(id);
            return NoContent();
        }
    }
}