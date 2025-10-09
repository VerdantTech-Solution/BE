//using AutoMapper;
//using BLL.DTO.Media;
//using BLL.DTO.Product;
//using BLL.DTO.ProductCategory;
//using BLL.DTO.ProductRegistration;
//using BLL.Interfaces;
//using DAL.Data;
//using DAL.Data.Models;
//using DAL.IRepository;
//using Microsoft.EntityFrameworkCore;

//namespace BLL.Services
//{
//    public class ProductService : IProductService
//    {
//        private readonly IProductRepository _productRepository;
//        private readonly IProductRegistrationRepository _productRegistrationRepository;
//        private readonly IProductCategoryRepository _productCategoryRepository;
//        private readonly VerdantTechDbContext _db;
//        private readonly IMapper _mapper;

//        public ProductService(
//            IProductRepository productRepository,
//            IProductRegistrationRepository productRegistrationRepository,
//            IProductCategoryRepository productCategoryRepository,
//            IMapper mapper,
//            VerdantTechDbContext db)
//        {
//            _productRepository = productRepository;
//            _productRegistrationRepository = productRegistrationRepository;
//            _productCategoryRepository = productCategoryRepository;
//            _mapper = mapper;
//            _db = db;
//        }

//        #region --- Product Registration ---
//        public async Task<ProductRegistrationReponseDTO> ProductRegistrationAsync(
//            ulong currentUserId,
//            ProductRegistrationCreateDTO requestDTO,
//            CancellationToken cancellationToken = default)
//        {
//            var category = await _productCategoryRepository.GetProductCategoryByIdAsync(requestDTO.CategoryId, true, cancellationToken);
//            if (category == null || !category.IsActive)
//                throw new KeyNotFoundException("Danh mục sản phẩm không hợp lệ");

//            if (currentUserId == 0)
//                throw new UnauthorizedAccessException("Người dùng chưa đăng nhập hoặc không hợp lệ");

//            var entity = _mapper.Map<ProductRegistration>(requestDTO);
//            entity.VendorId = currentUserId;
//            entity.CreatedAt = DateTime.UtcNow;

//            var created = await _productRegistrationRepository.CreateProductAsync(entity, cancellationToken);
//            return _mapper.Map<ProductRegistrationReponseDTO>(created);
//        }
//        #endregion

//        #region --- Product CRUD ---

//        public async Task<IReadOnlyList<ProductResponseDTO>> GetAllProductAsync(CancellationToken cancellationToken = default)
//        {
//            var list = await _productRepository.GetAllProductAsync(cancellationToken);
//            return _mapper.Map<IReadOnlyList<ProductResponseDTO>>(list);
//        }

//        public async Task<IReadOnlyList<ProductResponseDTO?>> GetAllProductByCategoryIdAsync(ulong id, CancellationToken cancellationToken = default)
//        {
//            var list = await _productRepository.GetAllProductByCategoryIdAsync(id, true, cancellationToken);
//            return _mapper.Map<IReadOnlyList<ProductResponseDTO?>>(list);
//        }

//        public async Task<ProductResponseDTO?> GetProductByIdAsync(ulong id, CancellationToken cancellationToken = default)
//        {
//            var product = await _productRepository.GetProductByIdAsync(id, true, cancellationToken);
//            if (product == null)
//                return null;

//            var response = _mapper.Map<ProductResponseDTO>(product);

//            var images = await _db.MediaLinks
//                .AsNoTracking()
//                .Where(m => m.OwnerType == MediaOwnerType.Product && m.OwnerId == id)
//                .OrderBy(m => m.SortOrder)
//                .Select(m => new ProductImageDTO
//                {
//                    Url = m.ImageUrl,
//                    PublicId = m.ImagePublicId,
//                    Purpose = m.Purpose.ToString(),
//                    SortOrder = m.SortOrder
//                })
//                .ToListAsync(cancellationToken);

//            response.Images = images;
//            return response;
//        }

//        public async Task<ProductResponseDTO> UpdateProductAsync(ulong id, ProductUpdateDTO dto, CancellationToken cancellationToken = default)
//        {
//            var product = await _productRepository.GetProductByIdAsync(id, false, cancellationToken);
//            if (product == null)
//                throw new KeyNotFoundException("Không tìm thấy sản phẩm");

//            var updated = _mapper.Map(dto, product);
//            var result = await _productRepository.UpdateProductAsync(updated, cancellationToken);
//            return _mapper.Map<ProductResponseDTO>(result);
//        }

//        #endregion

//        #region --- MediaLink handlers ---

//        public async Task AddProductImagesAsync(ulong productId, IReadOnlyList<MediaUploadDTO> uploads, CancellationToken ct)
//        {
//            if (uploads == null || uploads.Count == 0) return;

//            var exists = await _productRepository.GetProductByIdAsync(productId, true, ct);
//            if (exists == null)
//                throw new KeyNotFoundException("Sản phẩm không tồn tại");

//            var maxSort = await _db.MediaLinks
//                .Where(m => m.OwnerType == MediaOwnerType.Product && m.OwnerId == productId)
//                .Select(m => (int?)m.SortOrder)
//                .MaxAsync(ct) ?? -1;

//            var links = uploads.Select((u, idx) => new MediaLink
//            {
//                OwnerType = MediaOwnerType.Product,
//                OwnerId = productId,
//                ImageUrl = u.Url,
//                ImagePublicId = u.PublicId,
//                Purpose = MediaPurpose.None,
//                SortOrder = maxSort + 1 + idx,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            }).ToList();

//            await _db.MediaLinks.AddRangeAsync(links, ct);
//            await _db.SaveChangesAsync(ct);
//        }

//        public async Task ReplaceProductImagesAsync(ulong productId, IReadOnlyList<MediaUploadDTO> uploads, CancellationToken ct)
//        {
//            if (uploads == null || uploads.Count == 0) return;

//            var olds = await _db.MediaLinks
//                .Where(m => m.OwnerType == MediaOwnerType.Product && m.OwnerId == productId)
//                .ToListAsync(ct);

//            _db.MediaLinks.RemoveRange(olds);
//            await _db.SaveChangesAsync(ct);

//            var news = uploads.Select((u, idx) => new MediaLink
//            {
//                OwnerType = MediaOwnerType.Product,
//                OwnerId = productId,
//                ImageUrl = u.Url,
//                ImagePublicId = u.PublicId,
//                Purpose = MediaPurpose.None,
//                SortOrder = idx,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            }).ToList();

//            await _db.MediaLinks.AddRangeAsync(news, ct);
//            await _db.SaveChangesAsync(ct);
//        }

//        #endregion
//    }
//}
