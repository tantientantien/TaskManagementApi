using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!; // A unique key to hash the password
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
}