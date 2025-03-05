using TaskManagementApi.Dtos.Blob;

public interface IAzureService
{
    Task<TaskAttachmentDto> UploadAsync(IFormFile file);
    Task<bool> DeleteAsync(string blobFilename);
}