using System;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Task;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Mappers
{
    public static class TaskMapper
    {
        public static Task MapFromCreateDto(this TaskCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Task
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void MapFromUpdateDto(this TaskUpdateDto dto, Task existingTask)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (existingTask == null) throw new ArgumentNullException(nameof(existingTask));
            existingTask.Title = dto.Title;
            existingTask.Description = dto.Description;
            existingTask.IsCompleted = dto.IsCompleted;
            existingTask.UserId = dto.UserId;
            existingTask.CategoryId = dto.CategoryId;
        }

        public static TaskDataDto MapToDataDto(this Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            return new TaskDataDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                UserId = task.UserId,
                CategoryId = task.CategoryId,
                CreatedAt = task.CreatedAt
            };
        }
    }
}
