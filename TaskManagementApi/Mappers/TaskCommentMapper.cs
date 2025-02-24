using System;
using TaskManagementApi.Dtos.TaskComment;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class TaskCommentMapper
    {
        public static TaskComment MapFromCreateDto(TaskCommentCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new TaskComment
            {
                TaskId = dto.TaskId,
                UserId = dto.UserId,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void MapFromUpdateDto(TaskCommentUpdateDto dto, TaskComment existingComment)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (existingComment == null) throw new ArgumentNullException(nameof(existingComment));

            existingComment.Content = dto.Content;
        }

        public static TaskCommentDataDto MapToDataDto(TaskComment comment)
        {
            return new TaskCommentDataDto
            {
                TaskId = comment.TaskId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}