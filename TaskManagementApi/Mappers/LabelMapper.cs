using System;
using TaskManagementApi.Dtos.Label;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class LabelMapper
    {
        public static Label MapFromCreateDto(this LabelCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Label
            {
                Name = dto.Name,
            };
        }

        public static void MapFromUpdateDto(this LabelUpdateDto dto, Label existingLabel)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (existingLabel == null) throw new ArgumentNullException(nameof(existingLabel));

            existingLabel.Name = dto.Name;
        }

        public static LabelDataDto MapToDataDto(this Label label)
        {
            return new LabelDataDto
            {
                Id = label.Id,
                Name = label.Name
            };
        }

        public static IEnumerable<LabelDataDto> MapToDataDtoList(this IEnumerable<Label> labels)
        {
            return labels.Select(MapToDataDto);
        }
    }
}