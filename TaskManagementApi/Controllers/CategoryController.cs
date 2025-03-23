using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Dtos.CategoryDtos;
using TaskManagementApi.Dtos.Task;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET: api/categories
        [HttpGet]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieve a list of all available categories. Public access")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAll();
            if (!categories.Any())
                return NoContent();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDataDto>>(categories);
            return Ok(new { status = "success", message = "Get all categories successfully", data = categoryDtos });
        }

        // GET: api/categoris/:id
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get task by ID", Description = "Retrieves a task using its unique ID")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var categories = await _categoryRepository.GetById(id);
            var categoryDto = _mapper.Map<CategoryDataDto>(categories);
            return categories == null
                ? NotFound(new { status = "error", message = "Task not found" })
                : Ok(new { status = "success", message = "Task found", data = categoryDto });
        }

        // POST: api/categories
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create a new category", Description = "Add a new category to the system. Requires authentication")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "error", message = "Invalid category data", errors = ModelState });

            var category = _mapper.Map<Category>(createDto);
            await _categoryRepository.Add(category);

            var categoryDto = _mapper.Map<CategoryDataDto>(category);

            return CreatedAtAction(nameof(GetAllCategories), new { id = category.Id },
                new { status = "success", message = "Category created", data = categoryDto });
        }
    }
}