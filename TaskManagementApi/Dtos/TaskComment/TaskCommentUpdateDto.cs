using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.TaskComment
{
    public class TaskCommentUpdateDto
    {
        [Required(ErrorMessage = "Comment Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MaxLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string? Content { get; set; }
    }
}