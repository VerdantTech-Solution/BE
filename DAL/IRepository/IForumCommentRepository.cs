using DAL.Data;
using DAL.Data.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IForumCommentRepository
    {

        Task<ForumComment?> GetByIdAsync(ulong id, CancellationToken ct = default);
        // Lấy danh sách comment gốc theo PostId (có phân trang), tùy chọn kèm children (1 cấp).
        Task<(IReadOnlyList<ForumComment> Items, int TotalCount)> GetByPostIdAsync(ulong postId, int page, int pageSize, bool includeChildren = true, ForumCommentStatus? status = ForumCommentStatus.Visible, CancellationToken ct = default);
        //Lấy toàn bộ comment con trực tiếp của 1 comment cha
        Task<IReadOnlyList<ForumComment>> GetChildrenAsync(ulong parentId, ForumCommentStatus? status = ForumCommentStatus.Visible, CancellationToken ct = default);
        //Lấy tất cả comment của 1 user (có thể lọc theo status)
        Task<IReadOnlyList<ForumComment>> GetByUserIdAsync(ulong userId, ForumCommentStatus? status = null, CancellationToken ct = default);
        // Lấy nguyên thread: comment gốc + mọi hậu duệ (giới hạn độ sâu nếu cần)
        Task<IReadOnlyList<ForumComment>> GetThreadAsync(ulong rootCommentId, int? maxDepth = null, ForumCommentStatus? status = ForumCommentStatus.Visible, CancellationToken ct = default);

        //Tìm kiếm theo từ khóa + filter (postId, userId, status), có phân trang
        Task<(IReadOnlyList<ForumComment> Items, int TotalCount)> SearchAsync(string keyword, ulong? postId, ulong? userId, ForumCommentStatus? status, int page, int pageSize, CancellationToken ct = default);
        //Đếm số comment của 1 post (có filter theo status)
        Task<int> CountByPostAsync(ulong postId, ForumCommentStatus? status = ForumCommentStatus.Visible, CancellationToken ct = default);
        //Kiểm tra tồn tại comment theo Id
        Task<bool> ExistsAsync(ulong id, CancellationToken ct = default);

        Task AddAsync(ForumComment comment, CancellationToken ct = default);
        Task UpdateAsync(ForumComment comment, CancellationToken ct = default);
        //Xóa mềm: đổi trạng thái 
        Task SoftDeleteAsync(ulong id, CancellationToken ct = default);
        //Xóa khỏi DB
        Task DeleteAsync(ulong id, CancellationToken ct = default);
        //Đặt trạng thái comment (Visible/Hidden/... )
        Task<bool> SetStatusAsync(ulong id, ForumCommentStatus status, CancellationToken ct = default);
        //Tăng/giảm Like (atomic, update trực tiếp DB)
        Task<bool> IncrementLikeAsync(ulong id, int delta = 1, CancellationToken ct = default);
        //Tăng/giảm Dislike (atomic, update trực tiếp DB)
        Task<bool> IncrementDislikeAsync(ulong id, int delta = 1, CancellationToken ct = default);
        //Xóa mềm toàn bộ comment của 1 post (moderation)
        Task<int> BulkSoftDeleteByPostIdAsync(ulong postId, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
