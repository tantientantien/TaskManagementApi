using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Task;
using TaskManagementApi.Models;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Mappers
{
    public static class TaskMapper
    {
        public static Task ToTask(this TaskCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(nameof(dto));

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

        public static void UpdateTask(this TaskUpdateDto dto, Task existingTask)
        {
            ArgumentNullException.ThrowIfNull(nameof(dto));
            ArgumentNullException.ThrowIfNull(nameof(existingTask));

            existingTask.Title = dto.Title;
            existingTask.Description = dto.Description;
            existingTask.IsCompleted = dto.IsCompleted;
            existingTask.UserId = dto.UserId;
            existingTask.CategoryId = dto.CategoryId;
        }

        public static TaskDataDto ToDataDto(this Task task)
        {
            ArgumentNullException.ThrowIfNull(nameof(task));

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