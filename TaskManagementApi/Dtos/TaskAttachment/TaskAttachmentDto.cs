using System.Text.Json.Serialization;

namespace TaskManagementApi.Dtos.Blob
{
    public class TaskAttachmentDto
    {
        public int Id { get; set; }

        public string? FileName { get; set; }

        public string? FileUrl { get; set; }

        public int TaskId { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}