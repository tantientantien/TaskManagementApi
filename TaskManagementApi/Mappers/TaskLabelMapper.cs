using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class TaskLabelMapper
    {
        public static TaskLabel ToTaskLabel(this TaskLabelCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(nameof(dto));

            return new TaskLabel
            {
                TaskId = dto.TaskId,
                LabelId = dto.LabelId
            };
        }
    }
}