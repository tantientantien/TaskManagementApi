using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Dtos.Blob;
using TaskManagementApi.Models;

public class TaskAttachmentRepository : ITaskAttachmentRepository
{
    private readonly TaskManagementContext _context;
    private readonly IAzureService _azureStorage;
    private readonly IMapper _mapper;
    public TaskAttachmentRepository(TaskManagementContext context, IAzureService azureStorage, IMapper mapper)
    {
        _context = context;
        _azureStorage = azureStorage;
        _mapper = mapper;
    }

    public async Task<TaskAttachmentDto> Create(int taskId, IFormFile file)
    {
        var fileDto = await _azureStorage.UploadAsync(file);
        if (fileDto == null)
            throw new Exception("File upload failed.");

        var attachment = new TaskAttachment
        {
            TaskId = taskId,
            FileName = fileDto.FileName,
            FileUrl = fileDto.FileUrl,
            UploadedAt = DateTime.UtcNow
        };

        await _context.TaskAttachments.AddAsync(attachment);
        await _context.SaveChangesAsync();

        return _mapper.Map<TaskAttachmentDto>(attachment);
    }

    public async Task<TaskAttachmentDto?> GetById(int id)
    {
        var attachment = await _context.TaskAttachments
            .FirstOrDefaultAsync(a => a.Id == id);

        return attachment == null ? null : _mapper.Map<TaskAttachmentDto>(attachment);
    }
    public async Task<List<TaskAttachmentDto>> GetByTaskId(int taskId)
    {
        var attachments = await _context.TaskAttachments
            .Where(a => a.TaskId == taskId)
            .ToListAsync();

        return _mapper.Map<List<TaskAttachmentDto>>(attachments);
    }

    public async Task<TaskAttachmentDto> Update(TaskAttachmentDto taskAttachmentDto)
    {
        var attachment = await _context.TaskAttachments.FindAsync(taskAttachmentDto.Id);
        if (attachment == null)
            throw new KeyNotFoundException("Attachment not found.");

        _mapper.Map(taskAttachmentDto, attachment);
        _context.TaskAttachments.Update(attachment);
        await _context.SaveChangesAsync();

        return _mapper.Map<TaskAttachmentDto>(attachment);
    }

    public async Task<bool> Delete(int id)
    {
        var attachment = await _context.TaskAttachments.FindAsync(id);
        if (attachment == null)
            return false;

        await _azureStorage.DeleteAsync(attachment.FileUrl);
        _context.TaskAttachments.Remove(attachment);
        await _context.SaveChangesAsync();
        return true;
    }
}