using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ForumCommentRepository : IForumCommentRepository
    {
        private readonly VerdantTechDbContext _context;

        public ForumCommentRepository(VerdantTechDbContext context)
        {
            _context = context;
        }

        // ===== READ =====

        public async Task<ForumComment?> GetByIdAsync(ulong id, CancellationToken ct = default)
        {
            return await _context.ForumComments
                .Include(c => c.User)
                .Include(c => c.ForumPost)
                .Include(c => c.InverseParent)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<(IReadOnlyList<ForumComment> Items, int TotalCount)> GetByPostIdAsync(
            ulong postId,
            int page,
            int pageSize,
            bool includeChildren = true,
            ForumCommentStatus? status = ForumCommentStatus.Visible,
            CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var query = _context.ForumComments
                .Where(c => c.ForumPostId == postId && c.ParentId == null);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            var total = await query.CountAsync(ct);

            var baseQuery = query
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .AsNoTracking()
                .AsSplitQuery();

            if (includeChildren)
            {
                baseQuery = baseQuery
                    .Include(c => c.InverseParent.Where(ch => !status.HasValue || ch.Status == status.Value))
                    .ThenInclude(ch => ch.User);
            }

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<IReadOnlyList<ForumComment>> GetChildrenAsync(
            ulong parentId,
            ForumCommentStatus? status = ForumCommentStatus.Visible,
            CancellationToken ct = default)
        {
            var query = _context.ForumComments.Where(c => c.ParentId == parentId);
            if (status.HasValue) query = query.Where(c => c.Status == status.Value);

            return await query
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<ForumComment>> GetByUserIdAsync(
            ulong userId,
            ForumCommentStatus? status = null,
            CancellationToken ct = default)
        {
            var query = _context.ForumComments.Where(c => c.UserId == userId);
            if (status.HasValue) query = query.Where(c => c.Status == status.Value);

            return await query
                .Include(c => c.ForumPost)
                .OrderByDescending(c => c.CreatedAt)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<ForumComment>> GetThreadAsync(
            ulong rootCommentId,
            int? maxDepth = null,
            ForumCommentStatus? status = ForumCommentStatus.Visible,
            CancellationToken ct = default)
        {
            // BFS phía ứng dụng – đơn giản, DB-agnostic.
            // Nếu muốn 1 query: có thể thay bằng CTE đệ quy (SQL Server/MySQL) tuỳ bạn.
            var result = new List<ForumComment>();
            var queue = new Queue<(ulong id, int depth)>();

            var root = await _context.ForumComments
                .Include(c => c.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == rootCommentId &&
                                          (!status.HasValue || c.Status == status.Value), ct);

            if (root == null) return result;

            result.Add(root);
            queue.Enqueue((root.Id, 0));

            while (queue.Count > 0)
            {
                var (currentId, depth) = queue.Dequeue();
                if (maxDepth.HasValue && depth >= maxDepth.Value) continue;

                var children = await _context.ForumComments
                    .Where(c => c.ParentId == currentId && (!status.HasValue || c.Status == status.Value))
                    .Include(c => c.User)
                    .OrderBy(c => c.CreatedAt)
                    .AsNoTracking()
                    .ToListAsync(ct);

                result.AddRange(children);
                foreach (var ch in children)
                    queue.Enqueue((ch.Id, depth + 1));
            }

            return result;
        }

        public async Task<(IReadOnlyList<ForumComment> Items, int TotalCount)> SearchAsync(
            string keyword,
            ulong? postId,
            ulong? userId,
            ForumCommentStatus? status,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var q = _context.ForumComments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                q = q.Where(c => EF.Functions.Like(c.Content, $"%{keyword}%"));

            if (postId.HasValue) q = q.Where(c => c.ForumPostId == postId.Value);
            if (userId.HasValue) q = q.Where(c => c.UserId == userId.Value);
            if (status.HasValue) q = q.Where(c => c.Status == status.Value);

            var total = await q.CountAsync(ct);

            var items = await q
                .Include(c => c.User)
                .Include(c => c.ForumPost)
                .OrderByDescending(c => c.CreatedAt)
                .AsNoTracking()
                .AsSplitQuery()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<int> CountByPostAsync(
            ulong postId,
            ForumCommentStatus? status = ForumCommentStatus.Visible,
            CancellationToken ct = default)
        {
            var q = _context.ForumComments.Where(c => c.ForumPostId == postId);
            if (status.HasValue) q = q.Where(c => c.Status == status.Value);
            return await q.CountAsync(ct);
        }

        public async Task<bool> ExistsAsync(ulong id, CancellationToken ct = default)
        {
            return await _context.ForumComments.AnyAsync(c => c.Id == id, ct);
        }

        // ===== COMMANDS =====

        public async Task AddAsync(ForumComment comment, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            comment.CreatedAt = now;
            comment.UpdatedAt = now;
            await _context.ForumComments.AddAsync(comment, ct);
        }

        public Task UpdateAsync(ForumComment comment, CancellationToken ct = default)
        {
            comment.UpdatedAt = DateTime.UtcNow;
            _context.ForumComments.Update(comment);
            return Task.CompletedTask;
        }

        public async Task SoftDeleteAsync(ulong id, CancellationToken ct = default)
        {
            await _context.ForumComments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.Status, ForumCommentStatus.Hidden)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                    ct);
        }

        public async Task DeleteAsync(ulong id, CancellationToken ct = default)
        {
            var comment = await _context.ForumComments.FindAsync(new object?[] { id }, ct);
            if (comment != null)
            {
                _context.ForumComments.Remove(comment);
            }
        }

        public async Task<bool> SetStatusAsync(ulong id, ForumCommentStatus status, CancellationToken ct = default)
        {
            var affected = await _context.ForumComments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.Status, status)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                    ct);
            return affected > 0;
        }

        public async Task<bool> IncrementLikeAsync(ulong id, int delta = 1, CancellationToken ct = default)
        {
            var affected = await _context.ForumComments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.LikeCount, c => c.LikeCount + delta)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                    ct);
            return affected > 0;
        }

        public async Task<bool> IncrementDislikeAsync(ulong id, int delta = 1, CancellationToken ct = default)
        {
            var affected = await _context.ForumComments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.DislikeCount, c => c.DislikeCount + delta)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                    ct);
            return affected > 0;
        }

        public async Task<int> BulkSoftDeleteByPostIdAsync(ulong postId, CancellationToken ct = default)
        {
            return await _context.ForumComments
                .Where(c => c.ForumPostId == postId && c.Status != ForumCommentStatus.Hidden)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.Status, ForumCommentStatus.Hidden)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                    ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);
    }
}
