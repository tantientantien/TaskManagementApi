using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-labels")]
    [ApiController]
    public class TaskLabelController : ControllerBase
    {
        private readonly ITaskLabelRepository _taskLabelRepository;
        private readonly IGenericRepository<Task> _taskRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TaskLabelController(ITaskLabelRepository taskLabelRepository, IMapper mapper, UserManager<User> userManager, IGenericRepository<Task> taskRepository)
        {
            _taskLabelRepository = taskLabelRepository;
            _mapper = mapper;
            _userManager = userManager;
            _taskRepository = taskRepository;
        }

        // POST: api/task-labels
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Assign a label to a task", Description = "Requires authentication. Only the task owner or an admin can assign a label to a specific task.")]
        public async Task<IActionResult> AssignLabelToTask([FromBody] TaskLabelCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "error", message = "Invalid request data", errors = ModelState });

            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });

            var task = await _taskRepository.GetById(createDto.TaskId);
            if (task == null)
                return NotFound(new { status = "error", message = "Task not found" });

            if (!User.IsInRole("Admin") && userId != task.UserId)
                return Forbid();


            var storedLabel = await _taskLabelRepository.GetTaskLabelById(createDto.TaskId, createDto.LabelId);
            if (storedLabel != null)
                return Conflict(new { status = "error", message = "Label already assigned to task" });

            var taskLabel = _mapper.Map<TaskLabel>(createDto);
            await _taskLabelRepository.Add(taskLabel);

            return Ok(new { status = "success", message = "Task label assigned", data = _mapper.Map<TaskLabelDataDto>(taskLabel) });
        }


        // DELETE: api/task-labels/{taskId}/{labelId}
        [HttpDelete("{taskId:int}/{labelId:int}")]
        [Authorize]
        [SwaggerOperation(Summary = "Remove a label from a task", Description = "Requires authentication. Only the task owner or an admin can remove a label from a specific task.")]
        public async Task<IActionResult> RemoveLabelFromTask(int taskId, int labelId)
        {
            var userIdString = _userManager.GetUserId(User);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(new { status = "error", message = "Authentication required" });


            var taskLabel = await _taskLabelRepository.GetTaskLabelById(taskId, labelId);
            if (taskLabel == null)
                return NotFound(new { status = "error", message = "Task label not found" });

            if (!User.IsInRole("Admin") && userId != taskLabel?.Task?.UserId)
                return Forbid();

            await _taskLabelRepository.DeleteTaskLabel(taskId, labelId);
            return Ok(new { status = "success", message = "Task label removed" });
        }
    }
}