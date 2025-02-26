using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;
using TaskManagementApi.Repository;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-labels")]
    [ApiController]
    public class TaskLabelController : ControllerBase
    {
        private readonly IGenericRepository<TaskLabel> _taskLabelRepository;

        public TaskLabelController(IGenericRepository<TaskLabel> taskLabelRepository)
        {
            _taskLabelRepository = taskLabelRepository;
        }

        // POST: api/task-labels
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TaskLabel>> AssignLabelToATask([FromBody] TaskLabelCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid request data", errors = ModelState });
            }

            var taskLabel = TaskLabelMapper.ToTaskLabel(createDto);
            await _taskLabelRepository.Add(taskLabel);

            return Ok(new { status = "success", message = "Task label created", data = taskLabel });
        }

        // DELETE: api/task-labels/{taskId}/{labelId}
        [HttpDelete("{taskId:int}/{labelId:int}")]
        [Authorize]
        public async Task<IActionResult> RemoveLabelFromATask(int taskId, int labelId)
        {
            var taskLabel = await (_taskLabelRepository as TaskLabelRepository).GetById(taskId, labelId);
            if (taskLabel == null)
            {
                return NotFound(new { status = "error", message = "Task label not found" });
            }

            await (_taskLabelRepository as TaskLabelRepository).Delete(taskId, labelId);
            return Ok(new { status = "success", message = "Task label deleted" });
        }
    }
}