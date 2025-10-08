using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models;

namespace DAL.IRepository
{
    public interface IProductRegistrationRepository
    {
        Task<ProductRegistration> CreateAsync(ProductRegistration entity, CancellationToken cancellationToken = default);

        Task<ProductRegistration> UpdateAsync(ProductRegistration entity, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(ulong id, CancellationToken cancellationToken = default);

        Task<ProductRegistration?> GetByIdAsync(ulong id, bool noTracking = true, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<ProductRegistration> items, int total)> GetPagedAsync( int page, int pageSize, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<ProductRegistration> items, int total)> GetByVendorPagedAsync( ulong vendorId, int page, int pageSize, CancellationToken cancellationToken = default);

        Task<bool> ChangeStatusAsync(ulong id, ProductRegistrationStatus status, string? reason, ulong? approvedBy, CancellationToken cancellationToken = default);
    }
}
