using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class TaskComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; } = null!;
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        public string Content { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
