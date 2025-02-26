using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TaskManagementApi.Models
{
    public class User : IdentityUser
    {
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
}