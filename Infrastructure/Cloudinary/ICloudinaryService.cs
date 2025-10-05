using Microsoft.AspNetCore.Http;

namespace Infrastructure.Cloudinary;

public record UploadResultDto(string PublicId, string Url);

public interface ICloudinaryService
{
    Task<UploadResultDto?> UploadAsync(IFormFile file, string? folder = null, CancellationToken ct = default);
    Task<bool> DeleteAsync(string publicId, CancellationToken ct = default);
}
