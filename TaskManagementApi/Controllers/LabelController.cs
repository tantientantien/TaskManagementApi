using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<LabelDataDto>>> GetAllLabels()
        {
            var labels = await _labelRepository.GetAll();
            if (!labels.Any())
            {
                return Ok(new { status = "success", message = "No labels found" });
            }

            var data = LabelMapper.MapToDataDtoList(labels);
            return Ok(new { status = "success", message = "Labels retrieved", data });
        }

        // GET: api/labels/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LabelDataDto>> GetLabelById(int id)
        {
            var label = await _labelRepository.GetById(id);
            if (label == null)
            {
                return NotFound(new { status = "error", message = "Label not found" });
            }

            var data = LabelMapper.MapToDataDto(label);
            return Ok(new { status = "success", message = "Label found", data });
        }

        // POST: api/labels
        [HttpPost]
        public async Task<ActionResult<LabelDataDto>> AddLabel([FromBody] LabelCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = "error", message = "Invalid label data", errors = ModelState });
            }
            var existingLabels = await _labelRepository.GetAll();
            if (existingLabels.Any(l => l.Name == createDto.Name))
            {
                return Conflict(new { status = "error", message = "Label name already exists" });
            }

            var label = LabelMapper.MapFromCreateDto(createDto);
            await _labelRepository.Add(label);

            var labelDto = LabelMapper.MapToDataDto(label);
            return CreatedAtAction(nameof(GetLabelById), new { id = label.Id },
                new { status = "success", message = "Label created", data = labelDto });
        }

        // PUT: api/labels/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabel(int id, [FromBody] LabelUpdateDto updateDto)
        {
            if (!ModelState.IsValid || id != updateDto.Id)
            {
                return BadRequest(new { status = "error", message = "Label ID mismatch or invalid data", errors = ModelState });
            }

            var existingLabel = await _labelRepository.GetById(id);
            if (existingLabel == null)
            {
                return NotFound(new { status = "error", message = "Label not found" });
            }

            LabelMapper.MapFromUpdateDto(updateDto, existingLabel);
            await _labelRepository.Update(existingLabel);

            var updatedLabelDto = LabelMapper.MapToDataDto(existingLabel);
            return Ok(new { status = "success", message = "Label updated", data = updatedLabelDto });
        }

        // DELETE: api/labels/{id}
        [HttpDelete("{id}")]
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