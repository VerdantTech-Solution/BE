using Infrastructure.Cloudinary;
using Microsoft.AspNetCore.Mvc;

namespace Controller.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CloudinaryController : ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;

    public CloudinaryController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    /// <summary>
    /// Upload 1 ảnh — giống uploadService.uploadSingleImage('image')
    /// </summary>
    [HttpPost("upload-single")]
    [UploadSingleImage("image", "users")]
    public IActionResult UploadSingle()
    {
        var url = HttpContext.Items["uploadedImageUrl"] as string;
        var publicId = HttpContext.Items["uploadedImagePublicId"] as string;

        if (url == null)
            return BadRequest(new { message = "Không có ảnh được upload." });

        return Ok(new
        {
            message = "Upload ảnh thành công!",
            url,
            publicId
        });
    }

    /// <summary>
    /// Upload nhiều ảnh — giống uploadService.uploadManyImages('images')
    /// </summary>
    [HttpPost("upload-multiple")]
    [UploadMultipleImages("images", "products")]
    public IActionResult UploadMultiple()
    {
        var results = HttpContext.Items["uploadedImages"] as List<UploadResultDto>;
        if (results == null || results.Count == 0)
            return BadRequest(new { message = "Không có ảnh được upload." });

        return Ok(new
        {
            message = "Upload nhiều ảnh thành công!",
            images = results
        });
    }

    /// <summary>
    /// Xoá ảnh theo PublicId trên Cloudinary
    /// </summary>
    [HttpDelete("delete/{publicId}")]
    public async Task<IActionResult> DeleteImage(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            return BadRequest(new { message = "Thiếu publicId." });

        var success = await _cloudinaryService.DeleteAsync(publicId);
        if (!success)
            return BadRequest(new { message = "Xoá ảnh thất bại hoặc không tồn tại." });

        return Ok(new { message = "Đã xoá ảnh thành công.", publicId });
    }
}
