using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using BLL.Interfaces;                 // IForumCommentService (đổi nếu namespace khác)
using BLL.DTO;                        // APIResponse, PagedResponse<T>
using BLL.DTO.ForumComment;          // ForumComment DTOs

using DAL.Data.Models;               // ForumCommentStatus (nếu enum ở đây)
using DAL.Data;                      // ForumCommentStatus (nếu enum ở đây)

namespace Controller.Controllers;

[Route("api/[controller]")]
public class ForumCommentsController : BaseController
{
    private readonly IForumCommentService _service;

    public ForumCommentsController(IForumCommentService service)
    {
        _service = service;
    }

    // Helpers
    private (ulong? userId, bool isModerator) GetUserContext()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue("sub")
                   ?? User.FindFirstValue("uid");

        ulong? uid = null;
        if (ulong.TryParse(idStr, out var v)) uid = v;

        // Tùy hệ thống role: Admin/Staff là moderator
        var isMod = User.IsInRole("Admin") || User.IsInRole("Staff") || User.IsInRole("Moderator");
        return (uid, isMod);
    }

    private static void ClampPaging(ref int page, ref int pageSize, int max = 200)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > max) pageSize = max;
    }

    /// <summary>Tạo bình luận mới</summary>
    [HttpPost]
    [Authorize] // cần đăng nhập để tạo bình luận
    [EndpointSummary("Create Forum Comment")]
    [EndpointDescription("Tạo bình luận mới. parentId = null nếu là bình luận gốc. UserId lấy từ token.")]
    public async Task<ActionResult<APIResponse>> Create([FromBody] ForumCommentCreateDTO dto)
    {
        var validation = ValidateModel();
        if (validation != null) return validation;

        try
        {
            var (userId, _) = GetUserContext();
            if (userId is null) return ErrorResponse("Unauthorized", HttpStatusCode.Unauthorized);

            var created = await _service.CreateAsync(dto, userId.Value, GetCancellationToken());
            return SuccessResponse(created, HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Lấy chi tiết 1 bình luận theo Id</summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [EndpointSummary("Get Comment By Id")]
    [EndpointDescription("deep=false: trả kèm children 1 cấp; deep=true: trả nguyên thread nhiều cấp.")]
    public async Task<ActionResult<APIResponse>> GetById([FromRoute] ulong id, [FromQuery] bool deep = false)
    {
        try
        {
            var data = await _service.GetByIdAsync(id, deep, GetCancellationToken());
            if (data == null) return ErrorResponse($"Không tìm thấy bình luận id={id}", HttpStatusCode.NotFound);
            return SuccessResponse(data);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Lấy danh sách bình luận gốc của 1 bài viết (có phân trang)</summary>
    [HttpGet("by-post/{postId}")]
    [AllowAnonymous]
    [EndpointSummary("Get Comments By Post (Paged)")]
    [EndpointDescription("VD: /api/ForumComments/by-post/123?page=1&pageSize=20&deep=false")]
    public async Task<ActionResult<APIResponse>> GetByPost(
        [FromRoute] ulong postId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool deep = false)
    {
        try
        {
            ClampPaging(ref page, ref pageSize);
            var paged = await _service.GetByPostAsync(postId, page, pageSize, deep, GetCancellationToken());
            return SuccessResponse(paged);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Lấy các bình luận con trực tiếp của một bình luận</summary>
    [HttpGet("{id}/children")]
    [AllowAnonymous]
    [EndpointSummary("Get Direct Children")]
    [EndpointDescription("Trả về danh sách các comment là con trực tiếp của comment {id}.")]
    public async Task<ActionResult<APIResponse>> GetChildren([FromRoute] ulong id)
    {
        try
        {
            var items = await _service.GetChildrenAsync(id, GetCancellationToken());
            return SuccessResponse(items);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Tìm kiếm & lọc bình luận (có phân trang)</summary>
    [HttpGet("search")]
    [AllowAnonymous]
    [EndpointSummary("Search/Filter Comments (Paged)")]
    [EndpointDescription("Query: keyword, postId, userId, status (Visible/Hidden/Deleted), page, pageSize")]
    public async Task<ActionResult<APIResponse>> Search(
        [FromQuery] string? keyword,
        [FromQuery] ulong? postId,
        [FromQuery] ulong? userId,
        [FromQuery] ForumCommentStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            ClampPaging(ref page, ref pageSize);
            var paged = await _service.SearchAsync(keyword, postId, userId, status, page, pageSize, GetCancellationToken());
            return SuccessResponse(paged);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Cập nhật nội dung bình luận (không đổi trạng thái)</summary>
    [HttpPatch("{id}")]
    [Authorize]
    [EndpointSummary("Update Comment Content")]
    [EndpointDescription("Chỉ tác giả hoặc Admin/Staff mới được sửa nội dung.")]
    public async Task<ActionResult<APIResponse>> UpdateContent([FromRoute] ulong id, [FromBody] ForumCommentUpdateDTO dto)
    {
        var validation = ValidateModel();
        if (validation != null) return validation;

        try
        {
            var (uid, isMod) = GetUserContext();
            if (uid is null) return ErrorResponse("Unauthorized", HttpStatusCode.Unauthorized);

            var ok = await _service.UpdateContentAsync(id, dto, uid.Value, isMod, GetCancellationToken());
            if (!ok) return ErrorResponse("Forbidden or not found", HttpStatusCode.Forbidden);

            // trả về bản mới nhất
            var updated = await _service.GetByIdAsync(id, false, GetCancellationToken());
            return SuccessResponse(updated);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Đổi trạng thái bình luận (moderation)</summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin,Staff")]
    [EndpointSummary("Set Comment Status")]
    [EndpointDescription("Chỉ Admin/Staff. Body: { status: Visible|Hidden|Deleted }")]
    public async Task<ActionResult<APIResponse>> SetStatus([FromRoute] ulong id, [FromBody] ForumCommentSetStatusDTO body)
    {
        var validation = ValidateModel();
        if (validation != null) return validation;

        try
        {
            body.Id = id; // đảm bảo khớp route
            var ok = await _service.SetStatusAsync(body, 0, true, GetCancellationToken());
            if (!ok) return ErrorResponse("Not found or cannot change status", HttpStatusCode.Forbidden);

            var updated = await _service.GetByIdAsync(id, false, GetCancellationToken());
            return SuccessResponse(updated);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Xóa bình luận (soft/hard)</summary>
    [HttpDelete("{id}")]
    [Authorize]
    [EndpointSummary("Delete Comment")]
    [EndpointDescription("hardDelete=false: soft delete (đổi status). hardDelete=true: xóa cứng.")]
    public async Task<ActionResult<APIResponse>> Delete([FromRoute] ulong id, [FromQuery] bool hardDelete = false)
    {
        try
        {
            var (uid, isMod) = GetUserContext();
            if (uid is null) return ErrorResponse("Unauthorized", HttpStatusCode.Unauthorized);

            var ok = await _service.DeleteAsync(id, hardDelete, uid.Value, isMod, GetCancellationToken());
            if (!ok) return ErrorResponse("Forbidden or not found", HttpStatusCode.Forbidden);

            // Trả 204 No Content theo BaseController
            return SuccessResponse(null, HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Tăng/giảm lượt thích (like)</summary>
    [HttpPost("{id}/like")]
    [Authorize] // tùy yêu cầu – có thể AllowAnonymous
    [EndpointSummary("Like Comment")]
    [EndpointDescription("delta mặc định = 1. Có thể truyền -1 để hủy like.")]
    public async Task<ActionResult<APIResponse>> Like([FromRoute] ulong id, [FromQuery] int delta = 1)
    {
        try
        {
            var ok = await _service.LikeAsync(id, delta, GetCancellationToken());
            if (!ok) return ErrorResponse("Not found", HttpStatusCode.NotFound);

            var updated = await _service.GetByIdAsync(id, false, GetCancellationToken());
            return SuccessResponse(updated);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>Tăng/giảm lượt không thích (dislike)</summary>
    [HttpPost("{id}/dislike")]
    [Authorize] // tùy yêu cầu – có thể AllowAnonymous
    [EndpointSummary("Dislike Comment")]
    [EndpointDescription("delta mặc định = 1. Có thể truyền -1 để hủy dislike.")]
    public async Task<ActionResult<APIResponse>> Dislike([FromRoute] ulong id, [FromQuery] int delta = 1)
    {
        try
        {
            var ok = await _service.DislikeAsync(id, delta, GetCancellationToken());
            if (!ok) return ErrorResponse("Not found", HttpStatusCode.NotFound);

            var updated = await _service.GetByIdAsync(id, false, GetCancellationToken());
            return SuccessResponse(updated);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
