using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApi.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        [Required]
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();
    }
}