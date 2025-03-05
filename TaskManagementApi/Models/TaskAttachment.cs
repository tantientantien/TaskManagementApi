using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementApi.Models
{
    public class TaskAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        public string FileUrl { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}