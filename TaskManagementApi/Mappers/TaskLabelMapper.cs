using System;
using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class TaskLabelMapper
    {
        public static TaskLabel MapFromCreateDto(this TaskLabelCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new TaskLabel
            {
                TaskId = dto.TaskId,
                LabelId = dto.LabelId
            };
        }

    }
}