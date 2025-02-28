using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.CategoryDtos
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Category ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
        public string? Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }
    }
}