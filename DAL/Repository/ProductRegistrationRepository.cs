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
            _db.ProductRegistrations.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<ProductRegistration?> GetByIdAsync(ulong id, bool includeNavigation = true, CancellationToken ct = default)
        {
            IQueryable<ProductRegistration> q = _db.ProductRegistrations.AsQueryable();

            if (includeNavigation)
            {
                q = q
                    .Include(x => x.Vendor)
                    .Include(x => x.Category);
            }

            return await q.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<(List<ProductRegistration> Items, int Total)> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var q = _db.ProductRegistrations
                      .OrderByDescending(x => x.CreatedAt)
                      .AsNoTracking();

            var total = await q.CountAsync(ct);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            return (items, total);
        }

        public async Task<(List<ProductRegistration> Items, int Total)> GetByVendorPagedAsync(ulong vendorId, int page, int pageSize, CancellationToken ct = default)
        {
            var q = _db.ProductRegistrations
                .Where(x => x.VendorId == vendorId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking();

            var total = await q.CountAsync(ct);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            return (items, total);
        }

        public async Task<ProductRegistration> UpdateAsync(ProductRegistration entity, CancellationToken ct = default)
        {
            _db.ProductRegistrations.Update(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(ulong id, CancellationToken ct = default)
        {
            var e = await _db.ProductRegistrations.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return false;
            _db.ProductRegistrations.Remove(e);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ChangeStatusAsync(ulong id, ProductRegistrationStatus status, string? reason, ulong? approvedBy, CancellationToken ct = default)
        {
            var e = await _db.ProductRegistrations.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return false;

            e.Status = status;
            e.RejectionReason = status == ProductRegistrationStatus.Rejected ? (reason ?? "") : null;
            e.ApprovedBy = status == ProductRegistrationStatus.Approved ? approvedBy : null;
            e.ApprovedAt = status == ProductRegistrationStatus.Approved ? DateTime.UtcNow : null;
            e.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<ProductRegistration>> GetProductRegistrationByVendorIdAsync(ulong vendorId, bool useNoTracking = true, CancellationToken cancellationToken = default)
        {
            return await _repository.GetAllByFilterAsync(_repository => _repository.VendorId == vendorId, useNoTracking, cancellationToken);
        }

        public async Task<ProductRegistration> UpdateProductRegistrationAsync(ProductRegistration entity, CancellationToken cancellationToken = default)
        {
            return await _repository.UpdateAsync(entity, cancellationToken);
       }
        public async Task<ProductRegistration?> GetProductRegistrationByIdAsync(ulong id, bool useNoTracking = true, CancellationToken cancellationToken = default)
        {
            return await _repository.GetAsync(_repository => _repository.Id == id, useNoTracking, cancellationToken);
        }
    }
}
