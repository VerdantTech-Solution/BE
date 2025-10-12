using DAL.Data;
using DAL.Data.Models;

namespace DAL.IRepository
{
    public interface IProductRegistrationRepository
    {
        // CREATE / READ / UPDATE / DELETE
        Task<ProductRegistration> CreateAsync(ProductRegistration entity, CancellationToken ct = default);
        Task<ProductRegistration?> GetByIdAsync(ulong id, bool includeNavigation = true, CancellationToken ct = default);

        // paging
        Task<(List<ProductRegistration> Items, int Total)> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
        Task<(List<ProductRegistration> Items, int Total)> GetByVendorPagedAsync(ulong vendorId, int page, int pageSize, CancellationToken ct = default);

        Task<ProductRegistration> UpdateAsync(ProductRegistration entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(ulong id, CancellationToken ct = default);

        // status
        Task<bool> ChangeStatusAsync(ulong id, ProductRegistrationStatus status, string? reason, ulong? approvedBy, CancellationToken ct = default);
    }
}
