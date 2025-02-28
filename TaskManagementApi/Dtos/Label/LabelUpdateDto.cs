using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.Label
{
    public class LabelUpdateDto
    {
        [Required(ErrorMessage = "Label ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}