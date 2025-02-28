using Microsoft.AspNetCore.Identity;

namespace TaskManagementApi.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
}