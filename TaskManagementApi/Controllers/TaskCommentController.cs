using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TaskManagementApi.Dtos.TaskComment;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly IGenericRepository<TaskComment> _taskCommentRepository;
        private readonly UserManager<User> _userManager;

        public TaskCommentController(IGenericRepository<TaskComment> taskCommentRepository, UserManager<User> userManager)
        {
            _taskCommentRepository = taskCommentRepository;
            _userManager = userManager;
        }

        // POST: api/comments/{taskId}
        [HttpPost("{taskId}")]
        [Authorize]
        [SwaggerOperation(Summary = "Add a comment to a task", Description = "Requires authentication. Adds a new comment to the specified task")]
        public async Task<ActionResult<TaskCommentDataDto>> AddCommentToTask(int taskId, [FromBody] TaskCommentCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
            }

            var userId = _userManager.GetUserId(User);
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
        [SwaggerOperation(Summary = "Get a comment by ID", Description = "Retrieves a specific comment using its ID")]
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
        [Authorize(Roles = "Admin,User")]
        [SwaggerOperation(Summary = "Delete a comment", Description = "Only the comment author or an admin can delete a comment. Requires authentication")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { status = "error", message = "Comment not found" });
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId) || userId != comment.UserId)
            {
                return Forbid();
            }

            await _taskCommentRepository.Delete(id);
            return Ok(new { status = "success", message = "Comment deleted" });
        }
    }
}