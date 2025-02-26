using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [RegularExpression("^(User|Admin)$", ErrorMessage = "Role must be either 'User' or 'Admin'")]
        public string Role { get; set; } = "User";
    }
}