using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.TaskComment
{
    public class TaskCommentDataDto
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
