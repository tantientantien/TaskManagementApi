using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagementApi.Dtos.Label;
using TaskManagementApi.Mappers;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/labels")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly IGenericRepository<Label> _labelRepository;

        public LabelController(IGenericRepository<Label> labelRepository)
        {
            _labelRepository = labelRepository;
        }

        // GET: api/labels
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all labels",
                          Description = "Returns a list of all available labels. No authentication required")]
        public async Task<ActionResult<IEnumerable<LabelDataDto>>> GetAllLabels()
        {
            var labels = await _labelRepository.GetAll();
            if (!labels.Any())
            {
                return Ok(new { status = "success", message = "No labels found", data = new List<LabelDataDto>() });
            }

            var data = labels.Select(l => l.ToDataDto());
            return Ok(new { status = "success", message = "Labels retrieved", data });
        }

        // GET: api/labels/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieve a label by ID",
                          Description = "Fetches a specific label using its ID. No authentication required")]
        public async Task<ActionResult<LabelDataDto>> GetLabelById(int id)
        {
            var label = await _labelRepository.GetById(id);
            if (label == null)
            {
                return NotFound(new { status = "error", message = "Label not found" });
            }

            return Ok(new { status = "success", message = "Label found", data = label.ToDataDto() });
        }

        // POST: api/labels
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create a new label",
                          Description = "Allows authenticated users to create a new label. Label name must be unique")]
        public async Task<ActionResult<LabelDataDto>> AddLabel([FromBody] LabelCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid label data", errors = ModelState });
            }

            var storedLabels = await _labelRepository.GetAll();
            if (storedLabels.Any(l => l.Name == createDto.Name))
            {
                return Conflict(new { status = "error", message = "Label name already exists" });
            }

            var label = createDto.ToLabel();
            await _labelRepository.Add(label);

            return CreatedAtAction(nameof(GetLabelById), new { id = label.Id },
                new { status = "success", message = "Label created", data = label.ToDataDto() });
        }

        // PUT: api/labels/{id}
        [HttpPut("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Update an existing label",
                          Description = "Allows authenticated users to update an existing label's information")]
        public async Task<IActionResult> UpdateLabel(int id, [FromBody] LabelUpdateDto updateDto)
        {
            if (!ModelState.IsValid || id != updateDto.Id)
            {
                return BadRequest(new { status = "error", message = "Label ID mismatch or invalid data", errors = ModelState });
            }

            var storedLabel = await _labelRepository.GetById(id);
            if (storedLabel == null)
            {
                return NotFound(new { status = "error", message = "Label not found" });
            }

            updateDto.UpdateLabel(storedLabel);
            await _labelRepository.Update(storedLabel);

            return Ok(new { status = "success", message = "Label updated", data = storedLabel.ToDataDto() });
        }

        // DELETE: api/labels/{id}
        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete a label",
                          Description = "Allows authenticated users to delete a label by ID")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            var label = await _labelRepository.GetById(id);
            if (label == null)
            {
                return NotFound(new { status = "error", message = "Label not found" });
            }

            await _labelRepository.Delete(id);
            return Ok(new { status = "success", message = "Label deleted" });
        }
    }
}