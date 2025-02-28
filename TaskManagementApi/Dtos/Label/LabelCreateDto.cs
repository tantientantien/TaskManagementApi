using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.Label
{
    public class LabelCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }
    }
}