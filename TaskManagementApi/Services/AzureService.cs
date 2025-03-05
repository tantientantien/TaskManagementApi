using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TaskManagementApi.Dtos.Blob;

namespace AzureBlobStorage.WebApi.Repository
{
    public class AzureService : IAzureService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureService(IConfiguration configuration)
        {
            var connectionString = configuration["BlobConnectionString"];
            var containerName = configuration["BlobContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<TaskAttachmentDto?> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var uniqueFileName = GetUniqueFileName(file.FileName);
            BlobClient blobClient = _containerClient.GetBlobClient(uniqueFileName);
            await EnsureContainerExists();

            try
            {
                await using var stream = file.OpenReadStream();
                var options = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                };

                await blobClient.UploadAsync(stream, options);
                return new TaskAttachmentDto
                {
                    FileName = uniqueFileName,
                    FileUrl = blobClient.Uri.AbsoluteUri,
                    UploadedAt = DateTime.UtcNow
                };
            }
            catch (Exception ex) when (ex is RequestFailedException or IOException)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string blobFilename)
        {
            if (string.IsNullOrWhiteSpace(blobFilename))
                return false;

            try
            {
                var blobClient = _containerClient.GetBlobClient(blobFilename);
                var response = await blobClient.DeleteIfExistsAsync();
                return response.Value;
            }
            catch (Exception ex) when (ex is RequestFailedException or IOException)
            {
                return false;
            }
        }

        private async Task EnsureContainerExists()
        {
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        }

        private static string GetUniqueFileName(string fileName)
        {
            var uniqueId = Guid.NewGuid().ToString("N");
            return $"{Path.GetFileNameWithoutExtension(fileName)}_{uniqueId}{Path.GetExtension(fileName)}";
        }
    }
}