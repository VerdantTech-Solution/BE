using System.Threading;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.DTO.ProductRegistration;
using DAL.Data;
using DAL.Data.Models;

namespace BLL.Interfaces
{
    public interface IProductRegistrationService
    {
        // CREATE
        Task<ProductRegistrationReponseDTO> CreateAsync(ProductRegistrationCreateDTO dto, CancellationToken ct = default);

        // READS (có phân trang)
        Task<PagedResponse<ProductRegistrationReponseDTO>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
        Task<PagedResponse<ProductRegistrationReponseDTO>> GetByVendorAsync(ulong vendorId, int page, int pageSize, CancellationToken ct = default);
        Task<ProductRegistrationReponseDTO?> GetByIdAsync(ulong id, CancellationToken ct = default);

        // UPDATE
        Task<ProductRegistrationReponseDTO> UpdateAsync(ProductRegistrationUpdateDTO dto, CancellationToken ct = default);

        // CHANGE STATUS (duyệt / từ chối)
        Task<bool> ChangeStatusAsync(ulong id, ProductRegistrationStatus status, string? rejectionReason, ulong? approvedBy, CancellationToken ct = default);

        // DELETE
        Task<bool> DeleteAsync(ulong id, CancellationToken ct = default);
    }
}
