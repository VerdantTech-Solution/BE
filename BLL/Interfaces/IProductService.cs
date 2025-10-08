using BLL.DTO.Media;
using BLL.DTO.Product;
using BLL.DTO.ProductRegistration;
using BLL.DTO.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        Task<ProductRegistrationReponseDTO> ProductRegistrationAsync(ulong currentUserId, ProductRegistrationCreateDTO requestDTO, CancellationToken cancellationToken = default);        
        Task<ProductResponseDTO?> GetProductByIdAsync(ulong id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProductResponseDTO?>> GetAllProductByCategoryIdAsync(ulong id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProductResponseDTO>> GetAllProductAsync(CancellationToken cancellationToken = default);
        Task<ProductResponseDTO> UpdateProductAsync(ulong id, ProductUpdateDTO dto, CancellationToken cancellationToken = default);

        /// <summary>Gắn thêm ảnh vào product (không xoá ảnh cũ)</summary>
        Task AddProductImagesAsync(ulong productId, IReadOnlyList<MediaUploadDTO> uploads, CancellationToken ct);

        /// <summary>Thay toàn bộ ảnh product bằng danh sách mới (xoá cũ cả DB + Cloudinary)</summary>
        Task ReplaceProductImagesAsync(ulong productId, IReadOnlyList<MediaUploadDTO> uploads, CancellationToken ct);
    }
}
