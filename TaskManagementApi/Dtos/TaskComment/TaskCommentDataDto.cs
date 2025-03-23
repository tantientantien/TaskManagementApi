using TaskManagementApi.Dtos.User;

namespace TaskManagementApi.Dtos.TaskComment
{
    public class TaskCommentDataDto
    {
        public int id { get; set; }
        public UserDataDto? User { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
