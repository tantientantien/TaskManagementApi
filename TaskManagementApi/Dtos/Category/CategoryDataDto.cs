using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos.Category
{
    public class CategoryDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
