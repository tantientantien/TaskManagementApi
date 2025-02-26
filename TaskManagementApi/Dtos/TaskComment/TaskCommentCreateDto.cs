using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.TaskComment
{
    public class TaskCommentCreateDto
    {
        [Required(ErrorMessage = "TaskId is required.")]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MaxLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}