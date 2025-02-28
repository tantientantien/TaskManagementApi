namespace TaskManagementApi.Dtos.TaskComment
{
    public class TaskCommentDataDto
    {
        public int TaskId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
