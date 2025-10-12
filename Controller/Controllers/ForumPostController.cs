using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.DTO.ForumPost;
using BLL.DTO.Media;
using BLL.Interfaces;
using DAL.Data;
using Infrastructure.Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumPostController : ControllerBase
    {
        private readonly IForumPostService _service;
        private readonly ICloudinaryService _cloudinary;

        public ForumPostController(IForumPostService service, ICloudinaryService cloudinary)
        {
            _service = service;
            _cloudinary = cloudinary;
        }

        // ======== Reads (giữ nguyên) ========

        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Get all post")]
        public async Task<ActionResult<PagedResponse<ForumPostResponseDTO>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] ForumPostStatus? status = null,
            CancellationToken ct = default)
            => Ok(await _service.GetAllAsync(page, pageSize, status, ct));

        [HttpGet("category/{categoryId:long}")]
        [AllowAnonymous]
        [EndpointSummary("Get all post by forum category")]

        public async Task<ActionResult<PagedResponse<ForumPostResponseDTO>>> GetByCategory(
            long categoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] ForumPostStatus? status = null,
            CancellationToken ct = default)
            => Ok(await _service.GetByCategoryAsync((ulong)categoryId, page, pageSize, status, ct));

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<ActionResult<ForumPostResponseDTO>> GetById(long id, CancellationToken ct = default)
        {
            var post = await _service.GetByIdAsync((ulong)id, ct);
            return post is null ? NotFound() : Ok(post);
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        [EndpointSummary("Get post by slug")]

        public async Task<ActionResult<ForumPostResponseDTO>> GetBySlug(string slug, CancellationToken ct = default)
        {
            var post = await _service.GetBySlugAsync(slug, ct);
            return post is null ? NotFound() : Ok(post);
        }

        // ======== Single CREATE (multipart/form-data) ========
        [HttpPost]
        //[Authorize] // cần token để lấy UserId
        [Consumes("multipart/form-data")]
        [EndpointSummary("Create post")]
        public async Task<ActionResult<ForumPostResponseDTO>> Create([FromForm] ForumPostCreateDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryGetCurrentUserId(out var uid)) return Unauthorized("Không lấy được user id từ token.");
            dto.UserId = uid; // tránh lỗi FK user_id

            // Upload cover nếu có
            MediaUploadDTO? cover = null;
            if (dto.CoverImage != null)
            {
                var up = await _cloudinary.UploadAsync(dto.CoverImage, "forum-posts/cover", ct);
                cover = new MediaUploadDTO { PublicId = up.PublicUrl, Url = up.Url };
            }

            // Upload gallery nếu có
            var gallery = new List<MediaUploadDTO>();
            if (dto.Images != null && dto.Images.Count > 0)
            {
                var ups = await _cloudinary.UploadManyAsync(dto.Images, "forum-posts/images", ct);
                gallery = ups.Select(x => new MediaUploadDTO { PublicId = x.PublicUrl, Url = x.Url }).ToList();
            }

            var created = await _service.CreateAsync(dto, cover, gallery, ct);
            return Ok(created);
        }

        // ======== Single UPDATE (multipart/form-data) ========
        [HttpPut("{id:long}")]
        //[Authorize]
        [Consumes("multipart/form-data")]
        [EndpointSummary("Update post")]

        public async Task<ActionResult<ForumPostResponseDTO>> Update(long id, [FromForm] ForumPostUpdateDTO dto, CancellationToken ct = default)
        {
            if (id != (long)dto.Id) return BadRequest("Id không khớp");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Xoá file Cloudinary người dùng yêu cầu
            var removeIds = dto.RemovePublicIds ?? new List<string>();
            foreach (var pid in removeIds)
                if (!string.IsNullOrWhiteSpace(pid))
                    await _cloudinary.DeleteAsync(pid, ct);

            // Upload cover mới nếu có
            MediaUploadDTO? newCover = null;
            if (dto.CoverImage != null)
            {
                var up = await _cloudinary.UploadAsync(dto.CoverImage, "forum-posts/cover", ct);
                newCover = new MediaUploadDTO { PublicId = up.PublicUrl, Url = up.Url };
            }

            // Upload gallery mới nếu có
            var newGallery = new List<MediaUploadDTO>();
            if (dto.Images != null && dto.Images.Count > 0)
            {
                var ups = await _cloudinary.UploadManyAsync(dto.Images, "forum-posts/images", ct);
                newGallery = ups.Select(x => new MediaUploadDTO { PublicId = x.PublicUrl, Url = x.Url }).ToList();
            }

            var updated = await _service.UpdateAsync(dto, newCover, newGallery, removeIds, ct);
            return Ok(updated);
        }

        // ======== Delete (tuỳ chọn xoá file Cloudinary gửi kèm) ========
        [HttpDelete("{id:long}")]
        //[Authorize(Roles = "Admin,Staff")]
        [EndpointSummary("Delete post")]

        public async Task<ActionResult> Delete(long id, [FromBody] List<string>? cloudinaryPublicIds, CancellationToken ct = default)
        {
            if (cloudinaryPublicIds != null)
                foreach (var pid in cloudinaryPublicIds)
                    if (!string.IsNullOrWhiteSpace(pid))
                        await _cloudinary.DeleteAsync(pid, ct);

            var ok = await _service.DeleteAsync((ulong)id, ct);
            return ok ? NoContent() : NotFound();
        }

        // ===== helper =====
        private bool TryGetCurrentUserId(out ulong userId)
        {
            userId = 0;
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue("sub")
                      ?? User.FindFirstValue("uid")
                      ?? User.FindFirstValue("id");
            return ulong.TryParse(raw, out userId);
        }
    }
}
