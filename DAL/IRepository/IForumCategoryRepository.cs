using DAL.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.IRepository
{
    public interface IForumCategoryRepository
    {
   
        Task<ForumCategory?> GetByIdAsync(ulong id,bool useNoTracking = true,CancellationToken cancellationToken = default);
        // Tạo mới ForumCategory
        Task<ForumCategory> CreateAsync(ForumCategory category,CancellationToken cancellationToken = default);
        // Cập nhật ForumCategory
        Task<ForumCategory> UpdateAsync(ForumCategory category,CancellationToken cancellationToken = default);
        // Xóa ForumCategory theo Id
        Task<bool> DeleteAsync(ulong id,CancellationToken cancellationToken = default);
    }
}
