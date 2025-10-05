using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.DTO.ForumComment;
using DAL.Data;
               

namespace BLL.Services.Interfaces
{
    public interface IForumCommentService
    {
        // Create
        Task<ForumCommentResponseDTO> CreateAsync( ForumCommentCreateDTO dto, ulong currentUserId, CancellationToken ct = default);
        // Read
        Task<ForumCommentResponseDTO?> GetByIdAsync( ulong id, bool deep = false, CancellationToken ct = default);
        Task<PagedResponse<ForumCommentResponseDTO>> GetByPostAsync( ulong postId, int page, int pageSize, bool deep = false, CancellationToken ct = default);
        Task<IReadOnlyList<ForumCommentResponseDTO>> GetChildrenAsync( ulong parentId, CancellationToken ct = default);
        Task<PagedResponse<ForumCommentResponseDTO>> SearchAsync( string? keyword, ulong? postId, ulong? userId, ForumCommentStatus? status, int page, int pageSize, CancellationToken ct = default);
        Task<int> CountByPostAsync( ulong postId, ForumCommentStatus? status = ForumCommentStatus.Visible, CancellationToken ct = default);
        // Update content (không đổi status)
        Task<bool> UpdateContentAsync( ulong id, ForumCommentUpdateDTO dto, ulong currentUserId, bool isModerator = false, CancellationToken ct = default);
        // Moderation
        Task<bool> SetStatusAsync( ForumCommentSetStatusDTO dto, ulong actingUserId, bool isModerator = false, CancellationToken ct = default);
        // Delete
        Task<bool> DeleteAsync( ulong id, bool hardDelete, ulong actingUserId, bool isModerator = false, CancellationToken ct = default);
        // Reactions
        Task<bool> LikeAsync(ulong id, int delta = 1, CancellationToken ct = default);
        Task<bool> DislikeAsync(ulong id, int delta = 1, CancellationToken ct = default);
    }
}
