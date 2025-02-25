using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Dtos.CategoryDtos;
using TaskManagementApi.Models;


namespace TaskManagementApi.Mappers
{
    public static class CategoryMapper
    {
        public static Category MapFromCreateDto(this CategoryCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };
        }

        public static void MapFromUpdateDto(this CategoryUpdateDto dto, Category existingCategory)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (existingCategory == null) throw new ArgumentNullException(nameof(existingCategory));
            existingCategory.Name = dto.Name;
            existingCategory.Description = dto.Description;
        }

        public static CategoryDataDto MapToDataDto(this Category category)
        {
            return new CategoryDataDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public static IEnumerable<CategoryDataDto> MapToDataDtoList(this IEnumerable<Category> categories)
        {
            return categories.Select(MapToDataDto);
        }
    }
}