using TaskManagementApi.Dtos.Blob;
using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Dtos.Label;
using TaskManagementApi.Dtos.TaskComment;
using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Dtos.User;

namespace TaskManagementApi.Dtos.Task
{
    public class TaskDetailDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int userId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Duedate { get; set; }
        public CategoryDataDto? Category { get; set; }
        public UserDataDto? user { get; set; }
        public UserDataDto? Assignee { get; set; }
        public List<TaskAttachmentDto> Attachments { get; set; } = new();
        public List<TaskCommentDataDto> Comments { get; set; } = new();
        public List<LabelDataDto> Labels { get; set; } = new();
    }
}
