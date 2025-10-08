using DAL.Data;
using DAL.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.IRepository
{
    public interface IForumPostRepository
    {
        Task<ForumPost?> GetByIdAsync(ulong id, bool useNoTracking = true, CancellationToken cancellationToken = default, bool includeNavigation = false);
        Task<ForumPost?> GetBySlugAsync(string slug, bool useNoTracking = true, CancellationToken cancellationToken = default, bool includeNavigation = false);
        Task<(List<ForumPost> Items, int TotalCount)> GetAllAsync(int page,int pageSize,ForumPostStatus? statusFilter = null,bool useNoTracking = true,CancellationToken cancellationToken = default, bool includeNavigation = false);
        Task<(List<ForumPost> Items, int TotalCount)> GetByCategoryAsync( ulong forumCategoryId, int page, int pageSize, ForumPostStatus? statusFilter = null, bool useNoTracking = true, CancellationToken cancellationToken = default, bool includeNavigation = false);
        Task<ForumPost> CreateAsync(ForumPost post, CancellationToken cancellationToken = default);
        Task<ForumPost> UpdateAsync(ForumPost post, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(ulong id, CancellationToken cancellationToken = default);
    }
}
