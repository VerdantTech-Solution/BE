using DAL.Data;
using DAL.Data.Models;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ForumPostRepository : IForumPostRepository
    {
        private readonly VerdantTechDbContext _context;

        public ForumPostRepository(VerdantTechDbContext context)
        {
            _context = context;
        }

        public async Task<ForumPost?> GetByIdAsync(ulong id, bool useNoTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<ForumPost> query = _context.ForumPosts;
            if (useNoTracking) query = query.AsNoTracking();

            // Nếu muốn include quan hệ:
            // query = query.Include(p => p.ForumCategory)
            //              .Include(p => p.User);

            return await query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<ForumPost?> GetBySlugAsync( string slug, bool useNoTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<ForumPost> query = _context.ForumPosts;
            if (useNoTracking) query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
        }

        public async Task<(List<ForumPost> Items, int TotalCount)> GetAllAsync(int page, int pageSize, ForumPostStatus? statusFilter = null, bool useNoTracking = true, CancellationToken cancellationToken = default)
        {
            const int MaxPageSize = 100;
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;
            if (pageSize > MaxPageSize) pageSize = MaxPageSize;

            IQueryable<ForumPost> query = _context.ForumPosts;

            if (statusFilter.HasValue)
                query = query.Where(p => p.Status == statusFilter.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            query = query
                .OrderByDescending(p => p.IsPinned)
                .ThenByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            if (useNoTracking) query = query.AsNoTracking();

            var items = await query.ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<(List<ForumPost> Items, int TotalCount)> GetByCategoryAsync(ulong forumCategoryId, int page, int pageSize, ForumPostStatus? statusFilter = null, bool useNoTracking = true, CancellationToken cancellationToken = default)
        {
            const int MaxPageSize = 100;
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;
            if (pageSize > MaxPageSize) pageSize = MaxPageSize;

            IQueryable<ForumPost> query = _context.ForumPosts
                .Where(p => p.ForumCategoryId == forumCategoryId);

            if (statusFilter.HasValue)
            {
                query = query.Where(p => p.Status == statusFilter.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            query = query
                .OrderByDescending(p => p.IsPinned)
                .ThenByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            if (useNoTracking) query = query.AsNoTracking();

            var items = await query.ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<ForumPost> CreateAsync(ForumPost post, CancellationToken cancellationToken = default)
        {
            await _context.ForumPosts.AddAsync(post, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return post;
        }

        public async Task<ForumPost> UpdateAsync(ForumPost post, CancellationToken cancellationToken = default)
        {
            _context.ForumPosts.Update(post);
            await _context.SaveChangesAsync(cancellationToken);
            return post;
        }

        public async Task<bool> DeleteAsync(ulong id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.ForumPosts
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (entity == null) return false;

            _context.ForumPosts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
