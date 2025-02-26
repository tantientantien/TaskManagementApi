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

        // POST: api/comments/{taskId}
        [HttpPost("{taskId}")]
        [Authorize]
        public async Task<ActionResult<TaskCommentDataDto>> AddCommentToTask(int taskId, [FromBody] TaskCommentCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { status = "error", message = "Unauthorized: Missing userId" });
            }

            var comment = createDto.ToTaskComment();
            comment.UserId = userId;
            comment.TaskId = taskId;

            await _taskCommentRepository.Add(comment);

            var responseDto = comment.ToDataDto();

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id },
                new { status = "success", message = "Comment created", data = responseDto });
        }

        // GET: api/comments/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskCommentDataDto>> GetCommentById(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { status = "error", message = "Comment not found" });
            }

            return Ok(new { status = "success", message = "Comment found", data = comment.ToDataDto() });
        }

        // DELETE: api/comments/{id}
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { status = "error", message = "Comment not found" });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || userId != comment.UserId)
            {
                return Forbid();
            }

            await _taskCommentRepository.Delete(id);
            return Ok(new { status = "success", message = "Comment deleted" });
        }
    }
}