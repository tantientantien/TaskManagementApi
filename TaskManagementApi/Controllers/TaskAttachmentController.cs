using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Dtos.Blob;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Controllers
{
    [Route("api/attachments")]
    [ApiController]
    [Authorize]
    public class TaskAttachmentController : ControllerBase
    {
        private readonly ITaskAttachmentRepository _taskAttachmentRepository;
        private readonly IGenericRepository<Task> _taskRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TaskAttachmentController(ITaskAttachmentRepository taskAttachmentRepository, IGenericRepository<Task> taskRepository, IMapper mapper, UserManager<User> userManager)
        {
            _taskAttachmentRepository = taskAttachmentRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("upload/{taskId}")]
        [SwaggerOperation(Summary = "Upload an attachment", Description = "Uploads a file attachment for a specific task")]
        [RequestSizeLimit(104857600)] //100mb
        public async Task<IActionResult> UploadAttachment(int taskId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { status = "error", message = "Invalid file" });

            var task = await _taskRepository.GetById(taskId);
            if (task == null)
                return NotFound(new { status = "error", message = "Task not found" });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            if (task.UserId != userId)
                return Forbid();

            try
            {
                var attachment = await _taskAttachmentRepository.Create(taskId, file);
                return CreatedAtAction(nameof(GetAttachments), new { taskId },
                    new { status = "success", message = "Attachment uploaded", data = attachment });
            }
            catch (Exception ex)
            {
                return NotFound(new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("{taskId}")]
        [SwaggerOperation(Summary = "Get task attachments", Description = "Retrieves all attachments for a specific task")]
        public async Task<IActionResult> GetAttachments(int taskId)
        {
            var attachments = await _taskAttachmentRepository.GetByTaskId(taskId);
            if (attachments?.Count == 0)
                return NotFound(new { status = "error", message = "No attachments found for the task" });

            var attachmentDtos = _mapper.Map<List<TaskAttachmentDto>>(attachments);
            return Ok(new { status = "success", message = "Attachments retrieved", data = attachmentDtos });
        }

        [HttpDelete("{attachmentId}")]
        [SwaggerOperation(Summary = "Delete an attachment", Description = "Deletes a specific attachment by ID")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            var attachment = await _taskAttachmentRepository.GetById(attachmentId);
            if (attachment == null)
                return NotFound(new { status = "error", message = "Attachment not found" });

            var task = await _taskRepository.GetById(attachment.TaskId);
            if (task == null)
                return NotFound(new { status = "error", message = "Task not found" });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            if (task.UserId != userId)
                return Forbid();

            return await _taskAttachmentRepository.Delete(attachmentId)
                ? Ok(new { status = "success", message = "Attachment deleted" })
                : BadRequest(new { status = "error", message = "Failed to delete attachment" });
        }
    }
}