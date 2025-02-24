using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}