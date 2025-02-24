using TaskManagementApi.Dtos.Task;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Mappers
{
    public static class UserMapper
    {
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
