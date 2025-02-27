using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-labels")]
    [ApiController]
    public class TaskLabelController : ControllerBase
    {
        private readonly ITaskLabelRepository _taskLabelRepository;

        public TaskLabelController(ITaskLabelRepository taskLabelRepository)
        {
            _taskLabelRepository = taskLabelRepository;
        }

        // POST: api/task-labels
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Assign a label to a task", Description = "Requires authentication. Assigns a label to a specific task")]
        public async Task<ActionResult<TaskLabel>> AssignLabelToTask([FromBody] TaskLabelCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid request data", errors = ModelState });
            }

            var taskLabel = TaskLabelMapper.ToTaskLabel(createDto);
            await _taskLabelRepository.Add(taskLabel);

            return Ok(new { status = "success", message = "Task label assigned", data = taskLabel });
        }

        // DELETE: api/task-labels/{taskId}/{labelId}
        [HttpDelete("{taskId:int}/{labelId:int}")]
        [Authorize]
        [SwaggerOperation(Summary = "Remove a label from a task", Description = "Requires authentication. Removes a label from a specific task")]
        public async Task<IActionResult> RemoveLabelFromTask(int taskId, int labelId)
        {
            var taskLabel = await _taskLabelRepository.GetTaskLabelById(taskId, labelId);
            if (taskLabel == null)
            {
                return NotFound(new { status = "error", message = "Task label not found" });
            }

            await _taskLabelRepository.DeleteTaskLabel(taskId, labelId);
            return Ok(new { status = "success", message = "Task label removed" });
        }
    }
}