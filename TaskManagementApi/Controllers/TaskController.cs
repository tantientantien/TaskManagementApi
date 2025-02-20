using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<List<TaskItem>> GetAllTasks()
        {
            return Ok(_taskService.GetAllTasks());
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTaskById(int id)
        {
            try
            {
                return Ok(_taskService.GetTaskById(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] TaskItem task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title))
            {
                return BadRequest(new { message = "Task title is required." });
            }

            _taskService.AddTask(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskItem task)
        {
            if (task == null || task.Id != id)
            {
                return BadRequest(new { message = "Task ID mismatch." });
            }

            try
            {
                _taskService.UpdateTask(task);
                return Ok("Updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                _taskService.DeleteTask(id);
                return Ok("Deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}