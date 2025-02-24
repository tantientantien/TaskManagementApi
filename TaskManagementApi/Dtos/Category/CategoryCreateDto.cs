using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.CategoryDtos
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }
    }
}
