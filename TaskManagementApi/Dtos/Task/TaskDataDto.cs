using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Models;
using TaskManagementApi.Dtos.Blob;
using TaskManagementApi.Dtos.Label;
using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Dtos.User;

namespace TaskManagementApi.Dtos.Task
{
    public class TaskDataDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AttachmentCount { get; set; }
        public int CommentCount { get; set; }
        public int CategoryId { get; set; }
        public DateTime Duedate { get; set; }
        public List<TaskLabelDataDto> labels { get; set; } = new();
        public UserDataDto Assignee { get; set; } = new();
    }
}
