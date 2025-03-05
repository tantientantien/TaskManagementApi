using TaskManagementApi.Dtos.Blob;

public interface ITaskAttachmentRepository
{
    Task<TaskAttachmentDto> Create(int taskId, IFormFile file);
    Task<TaskAttachmentDto?> GetById(int id);
    Task<List<TaskAttachmentDto>> GetByTaskId(int taskId);
    Task<TaskAttachmentDto> Update(TaskAttachmentDto taskAttachmentDto);
    Task<bool> Delete(int id);
}