using System;
using TaskManagementApi.Dtos.TaskComment;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class TaskCommentMapper
    {
        public static TaskComment ToTaskComment(this TaskCommentCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            return new TaskComment
            {
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateTaskComment(this TaskCommentUpdateDto dto, TaskComment existingComment)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            ArgumentNullException.ThrowIfNull(existingComment, nameof(existingComment));

            existingComment.Content = dto.Content;
        }

        public static TaskCommentDataDto ToDataDto(this TaskComment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));
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