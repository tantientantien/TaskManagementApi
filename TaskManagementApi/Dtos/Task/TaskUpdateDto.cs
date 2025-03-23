using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos
{
    public class TaskUpdateDto
    {
        [MaxLength(200, ErrorMessage = "Title length cannot exceed 200 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Description length cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        public int AssigneeId { get; set; }
        public DateTime? Duedate { get; set; }

        public bool? IsCompleted { get; set; }

        public int? CategoryId { get; set; }
    }
}