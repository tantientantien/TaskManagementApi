using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.TaskComment
{
    public class TaskCommentCreateDto
    {
        [Required(ErrorMessage = "Content is required.")]
        [MaxLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}