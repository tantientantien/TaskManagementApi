using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Task;
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
        private readonly IMapper _mapper;

        public TaskController(
            IGenericRepository<Task> taskRepository,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/tasks
        [HttpGet]
        [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieves all tasks available in the system")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAll();
            if (!tasks.Any())
                return NoContent();

            var taskDtos = _mapper.Map<IEnumerable<TaskDataDto>>(tasks);
            return Ok(new { status = "success", message = "Tasks retrieved", data = taskDtos });
        }

        // GET: api/tasks/{id}
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get task by ID", Description = "Retrieves a task using its unique ID")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskRepository.GetById(id);
            var taskDto = _mapper.Map<TaskDataDto>(task);
            return task == null
                ? NotFound(new { status = "error", message = "Task not found" })
                : Ok(new { status = "success", message = "Task found", data = taskDto });
        }

        // POST: api/tasks
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create a new task", Description = "Requires authentication. Adds a new task to the system")]
        public async Task<IActionResult> AddTask([FromBody] TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "error", message = "Invalid task data", errors = ModelState });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            var task = _mapper.Map<Task>(taskDto);
            task.UserId = userId;
            await _taskRepository.Add(task);

            var taskDataDto = _mapper.Map<TaskDataDto>(task);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id },
                new { status = "success", message = "Task created", data = taskDataDto });
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        [SwaggerOperation(Summary = "Update a task", Description = "Only the task owner or an admin can update a task.")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });

            var storedTask = await _taskRepository.GetById(id);
            if (storedTask == null)
                return NotFound(new { status = "error", message = "Task not found" });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && userId != storedTask.UserId)
                return Forbid();

            _mapper.Map(taskDto, storedTask);
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
                return NotFound(new { status = "error", message = "Task not found" });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && userId != task.UserId)
                return Forbid();

            await _taskRepository.Delete(id);
            return Ok(new { status = "success", message = "Task deleted" });
        }
    }
}