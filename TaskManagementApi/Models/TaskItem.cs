using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    //public enum TaskStatus
    //{
    //    ToDo,
    //    InProgress,
    //    Completed
    //}

    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public bool Completed { get; set; } = false;

        public string? AssignedTo { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }

}
