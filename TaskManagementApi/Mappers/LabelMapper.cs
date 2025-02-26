using System;
using TaskManagementApi.Dtos.Label;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class LabelMapper
    {
        public static Label ToLabel(this LabelCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            return new Label
            {
                Name = dto.Name
            };
        }

        public static void UpdateLabel(this LabelUpdateDto dto, Label existingLabel)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            ArgumentNullException.ThrowIfNull(existingLabel, nameof(existingLabel));

            existingLabel.Name = dto.Name;
        }

        public static LabelDataDto ToDataDto(this Label label)
        {
            ArgumentNullException.ThrowIfNull(label, nameof(label));
            return new LabelDataDto
            {
                Id = label.Id,
                Name = label.Name
            };
        }
    }
}