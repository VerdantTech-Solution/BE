using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.Interfaces;
using BLL.DTO.ForumComment;
using BLL.DTO;
using DAL.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/forum-comments")]
    public class ForumCommentsController : ControllerBase
    {
        private readonly IForumCommentService _service;

        public ForumCommentsController(IForumCommentService service)
        {
            _service = service;
        }

        // Helper: đọc userId & role từ Claims
        private (ulong? userId, bool isModerator) GetUserContext()
        {
            // Tùy hệ thống auth, đổi NameIdentifier/"sub"/"uid" cho đúng
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue("sub")
                       ?? User.FindFirstValue("uid");

            ulong? uid = null;
            if (ulong.TryParse(idStr, out var parsed)) uid = parsed;

            var isMod = User.IsInRole("Admin") || User.IsInRole("Moderator");
            return (uid, isMod);
        }

        // =========== Create ===========
        [HttpPost]
        // [Authorize] // mở nếu cần bảo vệ
        public async Task<ActionResult<ForumCommentResponseDTO>> Create(
            [FromBody] ForumCommentCreateDTO dto,
            CancellationToken ct)
        {
            var (userId, _) = GetUserContext();
            if (userId is null) return Unauthorized();

            var res = await _service.CreateAsync(dto, userId.Value, ct);
            return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
        }

        // =========== Read ===========
        [HttpGet("{id:ulong}")]
        public async Task<ActionResult<ForumCommentResponseDTO>> GetById(
            [FromRoute] ulong id,
            [FromQuery] bool deep = false,
            CancellationToken ct = default)
        {
            var res = await _service.GetByIdAsync(id, deep, ct);
            if (res is null) return NotFound();
            return Ok(res);
        }

        // Lấy comment theo Post (paging)
        [HttpGet("by-post/{postId:ulong}")]
        public async Task<ActionResult<PagedResponse<ForumCommentResponseDTO>>> GetByPost(
            [FromRoute] ulong postId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] bool deep = false,
            CancellationToken ct = default)
        {
            var res = await _service.GetByPostAsync(postId, page, pageSize, deep, ct);
            return Ok(res);
        }

        // Children trực tiếp của 1 comment
        [HttpGet("{id:ulong}/children")]
        public async Task<ActionResult<IReadOnlyList<ForumCommentResponseDTO>>> GetChildren(
            [FromRoute] ulong id,
            CancellationToken ct = default)
        {
            var res = await _service.GetChildrenAsync(id, ct);
            return Ok(res);
        }

        // Search & filter (paging)
        [HttpGet("search")]
        public async Task<ActionResult<PagedResponse<ForumCommentResponseDTO>>> Search(
            [FromQuery] string? keyword,
            [FromQuery] ulong? postId,
            [FromQuery] ulong? userId,
            [FromQuery] ForumCommentStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var res = await _service.SearchAsync(keyword, postId, userId, status, page, pageSize, ct);
            return Ok(res);
        }

        // =========== Update ===========
        // Cập nhật NỘI DUNG (content) — không đổi status
        [HttpPatch("{id:ulong}")]
        // [Authorize]
        public async Task<IActionResult> UpdateContent(
            [FromRoute] ulong id,
            [FromBody] ForumCommentUpdateDTO dto,
            CancellationToken ct = default)
        {
            var (currentUserId, isModerator) = GetUserContext();
            if (currentUserId is null) return Unauthorized();

            var ok = await _service.UpdateContentAsync(id, dto, currentUserId.Value, isModerator, ct);
            if (!ok) return Forbid(); // hoặc NotFound() nếu muốn phân biệt
            return NoContent();
        }

        // Đổi STATUS (Moderation)
        [HttpPatch("{id:ulong}/status")]
        // [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> SetStatus(
            [FromRoute] ulong id,
            [FromBody] ForumCommentSetStatusDTO body,
            CancellationToken ct = default)
        {
            var (_, isModerator) = GetUserContext();
            // ép body.Id khớp id route (tránh client gửi sai)
            body.Id = id;

            var ok = await _service.SetStatusAsync(body, 0, isModerator, ct);
            if (!ok) return Forbid(); // hoặc NotFound nếu muốn
            return NoContent();
        }

        // =========== Delete ===========
        [HttpDelete("{id:ulong}")]
        // [Authorize]
        public async Task<IActionResult> Delete(
            [FromRoute] ulong id,
            [FromQuery] bool hardDelete = false,
            CancellationToken ct = default)
        {
            var (actingUserId, isModerator) = GetUserContext();
            if (actingUserId is null) return Unauthorized();

            var ok = await _service.DeleteAsync(id, hardDelete, actingUserId.Value, isModerator, ct);
            if (!ok) return Forbid(); // hoặc NotFound
            return NoContent();
        }

        // =========== Reactions ===========
        [HttpPost("{id:ulong}/like")]
        // [Authorize]
        public async Task<IActionResult> Like(
            [FromRoute] ulong id,
            [FromQuery] int delta = 1,
            CancellationToken ct = default)
        {
            var ok = await _service.LikeAsync(id, delta, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPost("{id:ulong}/dislike")]
        // [Authorize]
        public async Task<IActionResult> Dislike(
            [FromRoute] ulong id,
            [FromQuery] int delta = 1,
            CancellationToken ct = default)
        {
            var ok = await _service.DislikeAsync(id, delta, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
