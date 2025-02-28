using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos.Label;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/labels")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly IGenericRepository<Label> _labelRepository;
        private readonly IMapper _mapper;

        public LabelController(IGenericRepository<Label> labelRepository, IMapper mapper)
        {
            _labelRepository = labelRepository;
            _mapper = mapper;
        }

        // GET: api/labels
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all labels", Description = "Returns a list of all available labels. No authentication required")]
        public async Task<IActionResult> GetAllLabels()
        {
            var labels = await _labelRepository.GetAll();
            if (!labels.Any())
                return NoContent();

            var labelDtos = _mapper.Map<IEnumerable<LabelDataDto>>(labels);
            return Ok(new { status = "success", message = "Labels retrieved", data = labelDtos });
        }

        // GET: api/labels/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieve a label by ID", Description = "Fetches a specific label using its ID. No authentication required")]
        public async Task<IActionResult> GetLabelById(int id)
        {
            var label = await _labelRepository.GetById(id);
            var labelDto = _mapper.Map<LabelDataDto>(label);
            return label == null
                ? NotFound(new { status = "error", message = "Label not found" })
                : Ok(new { status = "success", message = "Label found", data = labelDto });
        }

        // POST: api/labels
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create a new label", Description = "Allows authenticated users to create a new label. Label name must be unique")]
        public async Task<IActionResult> AddLabel([FromBody] LabelCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "error", message = "Invalid label data", errors = ModelState });

            var label = _mapper.Map<Label>(createDto);
            await _labelRepository.Add(label);

            var labelDto = _mapper.Map<LabelDataDto>(label);

            return CreatedAtAction(nameof(GetLabelById), new { id = label.Id },
                new { status = "success", message = "Label created", data = labelDto });
        }

        // PUT: api/labels/{id}
        [HttpPut("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Update an existing label", Description = "Allows authenticated users to update an existing label's information")]
        public async Task<IActionResult> UpdateLabel(int id, [FromBody] LabelUpdateDto updateDto)
        {
            if (!ModelState.IsValid || id != updateDto.Id)
                return BadRequest(new { status = "error", message = "Label ID mismatch or invalid data", errors = ModelState });

            var storedLabel = await _labelRepository.GetById(id);
            if (storedLabel == null)
                return NotFound(new { status = "error", message = "Label not found" });

            _mapper.Map(updateDto, storedLabel);
            await _labelRepository.Update(storedLabel);

            var labelDto = _mapper.Map<LabelDataDto>(storedLabel);

            return Ok(new { status = "success", message = "Label updated", data = labelDto });
        }

        // DELETE: api/labels/{id}
        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete a label", Description = "Allows authenticated users to delete a label by ID")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            var label = await _labelRepository.GetById(id);
            if (label == null)
                return NotFound(new { status = "error", message = "Label not found" });

            await _labelRepository.Delete(id);
            return Ok(new { status = "success", message = "Label deleted" });
        }
    }
}