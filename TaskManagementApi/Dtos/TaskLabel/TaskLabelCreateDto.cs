using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.TaskLabel
{
    public class TaskLabelCreateDto
    {
        [Required(ErrorMessage = "TaskId is required.")]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "LabelId is required.")]
        public int LabelId { get; set; }
    }
}