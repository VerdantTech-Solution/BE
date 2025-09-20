using BLL.DTO;
using BLL.DTO.ForumPost;
using BLL.Interfaces;
using DAL.Data;
using DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumPostController : ControllerBase
    {
        private readonly IForumPostService _service;

        public ForumPostController(IForumPostService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Get all forum posts (paged)")]
        [EndpointDescription("Lấy tất cả bài viết, có phân trang và lọc theo trạng thái.")]
        public async Task<ActionResult<PagedResponse<ForumPostResponseDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] ForumPostStatus? status = null, CancellationToken ct = default)
        {
            var result = await _service.GetAllAsync(page, pageSize, status, ct);
            return Ok(result);
        }

        [HttpGet("category/{categoryId:long}")]
        [AllowAnonymous]
        [EndpointSummary("Get forum posts by category (paged)")]
        [EndpointDescription("Lấy tất cả bài viết theo category, có phân trang.")]
        public async Task<ActionResult<PagedResponse<ForumPostResponseDTO>>> GetByCategory(long categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] ForumPostStatus? status = null, CancellationToken ct = default)
        {
            var result = await _service.GetByCategoryAsync((ulong)categoryId, page, pageSize, status, ct);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        [EndpointSummary("Get forum post by ID")]
        [EndpointDescription("Lấy chi tiết một bài viết theo ID.")]
        public async Task<ActionResult<ForumPostResponseDTO>> GetById(long id, CancellationToken ct = default)
        {
            var post = await _service.GetByIdAsync((ulong)id, ct);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        [EndpointSummary("Get forum post by slug")]
        [EndpointDescription("Lấy chi tiết một bài viết theo slug.")]
        public async Task<ActionResult<ForumPostResponseDTO>> GetBySlug(string slug, CancellationToken ct = default)
        {
            var post = await _service.GetBySlugAsync(slug, ct);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        [EndpointSummary("Create forum post")]
        [EndpointDescription("Tạo mới một bài viết.")]
        public async Task<ActionResult<ForumPostResponseDTO>> Create([FromBody] ForumPostCreateDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto, ct);
            return Ok(created);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin,Staff")]
        [EndpointSummary("Update forum post")]
        [EndpointDescription("Cập nhật bài viết theo ID.")]
        public async Task<ActionResult<ForumPostResponseDTO>> Update(long id, [FromBody] ForumPostUpdateDTO dto, CancellationToken ct = default)
        {
            if (id != (long)dto.Id) return BadRequest("Id không khớp");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(dto, ct);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin,Staff")]
        [EndpointSummary("Delete forum post")]
        [EndpointDescription("Xóa một bài viết theo ID.")]
        public async Task<ActionResult> Delete(long id, CancellationToken ct = default)
        {
            var result = await _service.DeleteAsync((ulong)id, ct);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
