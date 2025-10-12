using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Cloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;
    private readonly CloudinaryOptions _opts;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    { "jpg","jpeg","png","gif","bmp","webp","tiff","ico","svg","pdf" };

    public CloudinaryService(IOptions<CloudinaryOptions> opts)
    {
        _opts = opts.Value;
        var account = new Account(_opts.CloudName, _opts.ApiKey, _opts.ApiSecret);
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    public async Task<UploadResultDto?> UploadAsync(IFormFile file, string? folder = null, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0) return null;

        var ext = Path.GetExtension(file.FileName)?.Trim('.').ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
            throw new InvalidOperationException($"Định dạng file không hợp lệ: {ext}");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = string.IsNullOrWhiteSpace(folder) ? _opts.DefaultFolder : $"{_opts.DefaultFolder}/{folder}".Trim('/'),
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false,
            Transformation = new Transformation()
                .Width(1000)
                .Height(1000)
                .Crop("limit")
                .Quality("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams, ct);
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
            return new UploadResultDto(result.PublicId!, result.SecureUrl?.ToString() ?? "");

        return null;
    }

    public async Task<IReadOnlyList<UploadResultDto>> UploadManyAsync(IEnumerable<IFormFile> files, string? folder = null, CancellationToken ct = default)
    {
        var outputs = new List<UploadResultDto>();
        foreach (var f in files)
        {
            var one = await UploadAsync(f, folder, ct);
            if (one is not null) outputs.Add(one);
        }
        return outputs;
    }

    public async Task<bool> DeleteAsync(string publicId, CancellationToken ct = default)
    {
        var param = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Image,  // Ảnh
            Type = "upload",                    // Mặc định loại "upload"
            Invalidate = true                   // Xoá cache CDN
        };

        var res = await _cloudinary.DestroyAsync(param);
        return string.Equals(res.Result, "ok", StringComparison.OrdinalIgnoreCase);
    }

}
