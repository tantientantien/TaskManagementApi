﻿using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TaskLabel>> AssignLabelToATask([FromBody] TaskLabelCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid request data", errors = ModelState });
            }

            var taskLabel = TaskLabelMapper.MapFromCreateDto(createDto);
            await _taskLabelRepository.Add(taskLabel);

            return Ok(new { status = "success", message = "Task label created", data = taskLabel });
        }

        // DELETE: api/task-labels/{taskId}/{labelId}
        [Authorize]
        [HttpDelete("{taskId:int}/{labelId:int}")]
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