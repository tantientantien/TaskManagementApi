using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApi.Dtos.TaskComment;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly IGenericRepository<TaskComment> _taskCommentRepository;

        public TaskCommentController(IGenericRepository<TaskComment> taskCommentRepository)
        {
            _taskCommentRepository = taskCommentRepository;
        }

        // POST: api/task-comments/{taskId}
        [Authorize]
        [HttpPost("{taskId}")]
        public async Task<ActionResult<TaskCommentDataDto>> AddCommentToATask(int taskId, [FromBody] TaskCommentCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { status = "error", message = "Unauthorized: Missing userId" });
            }

            var comment = TaskCommentMapper.MapFromCreateDto(createDto);
            comment.UserId = int.Parse(userIdClaim);
            comment.TaskId = taskId;

            await _taskCommentRepository.Add(comment);

            var responseDto = TaskCommentMapper.MapToDataDto(comment);

            return CreatedAtAction(nameof(GetTaskCommentById), new { id = comment.Id },
                new { status = "success", message = "Comment created", data = responseDto });
        }

        // GET: api/task-comments/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskCommentDataDto>> GetTaskCommentById(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { status = "error", message = "Comment not found" });
            }

            var responseDto = TaskCommentMapper.MapToDataDto(comment);

            return Ok(new { status = "success", message = "Comment found", data = responseDto });
        }

        // DELETE: api/task-comments/{id}
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { status = "error", message = "Comment not found" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || int.Parse(userIdClaim) != comment.UserId)
            {
                return Forbid();
            }

            await _taskCommentRepository.Delete(id);
            return NoContent();
        }
    }
}