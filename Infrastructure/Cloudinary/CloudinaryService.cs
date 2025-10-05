using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Cloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;
    private readonly CloudinaryOptions _opts;

    public CloudinaryService(IOptions<CloudinaryOptions> opts)
    {
        _opts = opts.Value;
        var account = new Account(_opts.CloudName, _opts.ApiKey, _opts.ApiSecret);
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    public async Task<UploadResultDto?> UploadAsync(IFormFile file, string? folder = null, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0) return null;

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = string.IsNullOrWhiteSpace(folder) ? _opts.DefaultFolder : folder,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams, ct);
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
            return new UploadResultDto(result.PublicId!, result.SecureUrl?.ToString() ?? "");

        return null;
    }

    public async Task<bool> DeleteAsync(string publicId, CancellationToken ct = default)
    {
        var res = await _cloudinary.DestroyAsync(new DeletionParams(publicId)); 
        return res.Result == "ok";
    }

}
