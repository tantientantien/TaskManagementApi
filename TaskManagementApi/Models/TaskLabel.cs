using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApi.Models
{
    public class TaskLabel
    {
        [Key]
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task? Task { get; set; }
        [Key]
        public int LabelId { get; set; }
        [ForeignKey("LabelId")]
        public Label? Label { get; set; }
    }
}