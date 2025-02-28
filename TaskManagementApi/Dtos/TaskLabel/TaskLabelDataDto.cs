using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.TaskLabel
{
    public class TaskLabelDataDto
    {
        public int TaskId { get; set; }
        public int LabelId { get; set; }
    }
}
