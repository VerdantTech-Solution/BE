using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class ProductRegistrationRepository : IProductRegistrationRepository
    {
        private readonly VerdantTechDbContext _db;
        public ProductRegistrationRepository(VerdantTechDbContext db) => _db = db;

        public async Task<ProductRegistration> CreateAsync(ProductRegistration entity, CancellationToken ct = default)
        {
            await _db.ProductRegistrations.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<ProductRegistration> UpdateAsync(ProductRegistration entity, CancellationToken ct = default)
        {
            _db.ProductRegistrations.Update(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(ulong id, CancellationToken ct = default)
        {
            var pr = await _db.ProductRegistrations.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (pr == null) return false;

            // xóa gallery liên quan
            var medias = _db.MediaLinks.Where(m => m.OwnerType == MediaOwnerType.ProductRegistration && m.OwnerId == id);
            _db.MediaLinks.RemoveRange(medias);

            _db.ProductRegistrations.Remove(pr);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<ProductRegistration?> GetByIdAsync(ulong id, bool noTracking = true, CancellationToken ct = default)
        {
            var q = _db.ProductRegistrations.AsQueryable();
            if (noTracking) q = q.AsNoTracking();
            return await q.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<(IReadOnlyList<ProductRegistration> items, int total)> GetPagedAsync( int page, int pageSize, CancellationToken ct = default)
        {
            var q = _db.ProductRegistrations.AsNoTracking().OrderByDescending(x => x.CreatedAt);
            var total = await q.CountAsync(ct);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
            return (items, total);
        }

        public async Task<(IReadOnlyList<ProductRegistration> items, int total)> GetByVendorPagedAsync( ulong vendorId, int page, int pageSize, CancellationToken ct = default)
        {
            var q = _db.ProductRegistrations.AsNoTracking()
                .Where(x => x.VendorId == vendorId)
                .OrderByDescending(x => x.CreatedAt);

            var total = await q.CountAsync(ct);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
            return (items, total);
        }

        public async Task<bool> ChangeStatusAsync(ulong id, ProductRegistrationStatus status, string? reason, ulong? approvedBy, CancellationToken ct = default)
        {
            var pr = await _db.ProductRegistrations.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (pr == null) return false;

            pr.Status = status;
            pr.RejectionReason = status == ProductRegistrationStatus.Rejected ? reason : null;
            pr.ApprovedBy = status == ProductRegistrationStatus.Approved ? approvedBy : null;
            pr.ApprovedAt = status == ProductRegistrationStatus.Approved ? DateTime.UtcNow : null;
            pr.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
