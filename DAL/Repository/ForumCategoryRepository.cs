using DAL.Data;
using DAL.Data.Models;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ForumCategoryRepository : IForumCategoryRepository
    {
        private readonly VerdantTechDbContext _context;

        public ForumCategoryRepository(VerdantTechDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<ForumCategory>> GetAllAsync( bool useNoTracking = true, CancellationToken cancellationToken = default)
        {
            var query = _context.ForumCategories.AsQueryable();
            if (useNoTracking) query = query.AsNoTracking();

            // Sắp xếp an toàn theo Id (nếu có trường SortOrder bạn có thể thay thế)
            return await query
                .OrderBy(c => c.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<ForumCategory?> GetByIdAsync(ulong id,bool useNoTracking = true,CancellationToken cancellationToken = default)
        {
            var query = _context.ForumCategories.AsQueryable();
            if (useNoTracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        // Tạo mới ForumCategory
        public async Task<ForumCategory> CreateAsync(ForumCategory category,CancellationToken cancellationToken = default)
        {
            await _context.ForumCategories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return category;
        }

        // Cập nhật ForumCategory
        public async Task<ForumCategory> UpdateAsync(ForumCategory category,CancellationToken cancellationToken = default)
        {
            _context.ForumCategories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);
            return category;
        }

        // Xóa ForumCategory theo Id
        public async Task<bool> DeleteAsync(ulong id,CancellationToken cancellationToken = default)
        {
            var category = await _context.ForumCategories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
                return false;

            _context.ForumCategories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
