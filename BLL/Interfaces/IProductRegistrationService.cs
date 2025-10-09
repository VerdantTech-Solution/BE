using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.DTO.Media;
using BLL.DTO.MediaLink;
using BLL.DTO.ProductRegistration;

namespace BLL.Interfaces
{
    public interface IProductRegistrationService
    {
        Task<ProductRegistrationReponseDTO> UpdateAsync(ProductRegistrationUpdateDTO dto, string? manualUrl, string? manualPublicUrl, IReadOnlyList<MediaLinkItemDTO> addImages, IReadOnlyList<string> removeImagePublicIds, CancellationToken ct = default);

        Task<ProductRegistrationReponseDTO> CreateAsync(ProductRegistrationCreateDTO dto, string? manualUrl, string? manualPublicUrl, IReadOnlyList<MediaLinkItemDTO> images, CancellationToken ct = default);
        Task<bool> DeleteAsync(ulong id, CancellationToken ct = default);

        Task<ProductRegistrationReponseDTO?> GetByIdAsync(ulong id, CancellationToken ct = default);

        Task<PagedResponse<ProductRegistrationReponseDTO>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);

        Task<PagedResponse<ProductRegistrationReponseDTO>> GetByVendorAsync(ulong vendorId, int page, int pageSize, CancellationToken ct = default);

        Task<bool> ChangeStatusAsync(ulong id, string status, string? reason, ulong? approvedBy, CancellationToken ct = default);
    }
}
