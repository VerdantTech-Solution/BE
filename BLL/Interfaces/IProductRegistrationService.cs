// BLL/Interfaces/IProductRegistrationService.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.DTO.Media;
using BLL.DTO.MediaLink;
using BLL.DTO.ProductRegistration;
using DAL.Data;
using DAL.Data.Models;

namespace BLL.Interfaces
{
    public interface IProductRegistrationService
    {
        Task<ProductRegistrationReponseDTO> CreateAsync( ProductRegistrationCreateDTO dto, string? manualUrl, string? manualPublicUrl, IReadOnlyList<MediaUploadDTO> images, IReadOnlyList<MediaUploadDTO> certificates, CancellationToken ct = default);
        Task<PagedResponse<ProductRegistrationReponseDTO>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
        Task<PagedResponse<ProductRegistrationReponseDTO>> GetByVendorAsync(ulong vendorId, int page, int pageSize, CancellationToken ct = default);
        Task<ProductRegistrationReponseDTO?> GetByIdAsync(ulong id, CancellationToken ct = default);
        Task<ProductRegistrationReponseDTO> UpdateAsync( ProductRegistrationUpdateDTO dto, string? manualUrl, string? manualPublicUrl, IReadOnlyList<MediaUploadDTO> addImages, IReadOnlyList<string> removeImagePublicIds, IReadOnlyList<MediaUploadDTO> addCertificates, IReadOnlyList<string> removeCertificatePublicIds, CancellationToken ct = default);
        Task<bool> ChangeStatusAsync(ulong id, ProductRegistrationStatus status, string? reason, ulong? approvedBy, CancellationToken ct = default);
        Task<bool> DeleteAsync(ulong id, CancellationToken ct = default);
    }
}
