using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.Task
{
    public class TaskDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
