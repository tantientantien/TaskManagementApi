using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TaskManagementApi.Dtos.TaskComment;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly IGenericRepository<TaskComment> _taskCommentRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public TaskCommentController(
            IGenericRepository<TaskComment> taskCommentRepository,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _taskCommentRepository = taskCommentRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        // POST: api/comments/{taskId}
        [HttpPost("{taskId}")]
        [Authorize]
        [SwaggerOperation(Summary = "Add a comment to a task", Description = "Requires authentication. Adds a new comment to the specified task")]
        public async Task<IActionResult> AddCommentToTask(int taskId, [FromBody] TaskCommentCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            var comment = _mapper.Map<TaskComment>(createDto);
            comment.UserId = userId;
            comment.TaskId = taskId;
            await _taskCommentRepository.Add(comment);

            var commentDto = _mapper.Map<TaskCommentDataDto>(comment);

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id },
                new { status = "success", message = "Comment created", data = commentDto });
        }

        // GET: api/comments/{id}
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get a comment by ID", Description = "Retrieves a specific comment using its ID")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            var commentDto = _mapper.Map<TaskCommentDataDto>(comment);
            return comment == null
                ? NotFound(new { status = "error", message = "Comment not found" })
                : Ok(new { status = "success", message = "Comment found", data = commentDto });
        }

        // DELETE: api/comments/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        [SwaggerOperation(Summary = "Delete a comment", Description = "Only the comment author or an admin can delete a comment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            if (comment == null)
                return NotFound(new { status = "error", message = "Comment not found" });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && userId != comment.UserId)
                return Forbid();

            await _taskCommentRepository.Delete(id);
            return Ok(new { status = "success", message = "Comment deleted" });
        }
    }
}