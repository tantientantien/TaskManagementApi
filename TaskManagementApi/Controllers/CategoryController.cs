using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Dtos.CategoryDtos;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryController(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDataDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAll();
            if (!categories.Any())
            {
                return NoContent();
            }

            var data = categories.Select(c => c.ToDataDto());
            return Ok(new { status = "success", message = "Get all categories successfully", data });
        }

        // POST: api/categories
        [HttpPost]
        [Authorize] // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDataDto>> AddCategory([FromBody] CategoryCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid category data", errors = ModelState });
            }

            var existingCategories = await _categoryRepository.GetAll();
            if (existingCategories.Any(c => c.Name == createDto.Name))
            {
                return Conflict(new { status = "error", message = "Category name already exists" });
            }

            var category = createDto.ToCategory();
            await _categoryRepository.Add(category);

            var categoryDto = category.ToDataDto();
            return CreatedAtAction(nameof(GetAllCategories), new { id = categoryDto.Id },
                new { status = "success", message = "Category created", data = categoryDto });
        }
    }
}