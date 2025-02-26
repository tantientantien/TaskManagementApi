using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Dtos.CategoryDtos;
using TaskManagementApi.Models;

public static class CategoryMapper
{
    public static Category ToCategory(this CategoryCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
        return new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }

    public static void UpdateCategory(this CategoryUpdateDto dto, Category existingCategory)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
        ArgumentNullException.ThrowIfNull(existingCategory, nameof(existingCategory));

        existingCategory.Name = dto.Name;
        existingCategory.Description = dto.Description;
    }

    public static CategoryDataDto ToDataDto(this Category category)
    {
        ArgumentNullException.ThrowIfNull(category, nameof(category));
        return new CategoryDataDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }
}