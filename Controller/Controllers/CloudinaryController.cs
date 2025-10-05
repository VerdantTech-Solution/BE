using Infrastructure.Cloudinary;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO.Upload;

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

    [HttpPost("upload-single")]
    [Consumes("multipart/form-data")]
    [UploadSingleImage("Image", "users")]
    public IActionResult UploadSingle([FromForm] SingleImageForm form)
    {
        var url = HttpContext.Items["uploadedImageUrl"] as string;
        var publicId = HttpContext.Items["uploadedImagePublicId"] as string;

        if (string.IsNullOrEmpty(url))
            return BadRequest(new { message = "Không có ảnh được upload." });

        return Ok(new { message = "Upload ảnh thành công!", url, publicId });
    }

    [HttpPost("upload-multiple")]
    [Consumes("multipart/form-data")]
    [UploadMultipleImages("Images", "products")]
    public IActionResult UploadMultiple([FromForm] MultipleImagesForm form)
    {
        var uploaded = HttpContext.Items["uploadedImages"] as List<UploadResultDto>;
        if (uploaded is null || uploaded.Count == 0)
            return BadRequest(new { message = "Không có ảnh được upload." });

        return Ok(new { message = "Upload nhiều ảnh thành công!", images = uploaded });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteImage([FromQuery] string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            return BadRequest(new { message = "Thiếu publicId." });

        var decoded = Uri.UnescapeDataString(publicId);

        var ok = await _cloudinaryService.DeleteAsync(decoded);
        return ok
            ? Ok(new { message = "Đã xoá ảnh thành công.", publicId = decoded })
            : BadRequest(new { message = "Xoá ảnh thất bại hoặc không tồn tại.", publicId = decoded });
    }

}
