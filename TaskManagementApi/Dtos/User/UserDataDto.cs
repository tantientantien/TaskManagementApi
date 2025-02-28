using System.Text.Json.Serialization;

namespace TaskManagementApi.Dtos.User
{
    public class UserDataDto
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
