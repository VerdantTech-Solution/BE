using BLL.DTO;
using BLL.DTO.Media;
using BLL.DTO.Product;
using BLL.DTO.ProductRegistration;
using BLL.Interfaces;
using DAL.Data;
using DAL.Data.Models;
using Infrastructure.Cloudinary;               // UploadMultipleImages + ICloudinaryService
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly VerdantTechDbContext _db;      
        private readonly ICloudinaryService _cloud;     

        public ProductController(IProductService productService,
                                 VerdantTechDbContext db,
                                 ICloudinaryService cloud)
        {
            _productService = productService;
            _db = db;
            _cloud = cloud;
        }

        /// <summary>
        /// Đăng ký sản phẩm mới từ nhà cung cấp (đợi phê duyệt)
        /// </summary>
        [HttpPost("register-product")]
        public async Task<IActionResult> RegisterProduct([FromBody] ProductRegistrationCreateDTO requestDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var vendorid = GetCurrentUserId();
            var result = await _productService.ProductRegistrationAsync(vendorid, requestDTO, cancellationToken);
            return Ok(result);
        }

        /// <summary> Lấy thông tin sản phẩm theo ID </summary>
        [HttpGet("{id}")]
        [EndpointSummary("Get Product By ID")]
        [EndpointDescription("Lấy thông tin  sản phẩm theo ID.")]
        public async Task<ActionResult<APIResponse>> GetProductById([FromRoute] ulong id)
        {
            try
            {
                var result = await _productService.GetProductByIdAsync(id, GetCancellationToken());
                if (result == null)
                    return ErrorResponse($"Không tìm thấy sản phẩm với ID {id}", HttpStatusCode.NotFound);
                return SuccessResponse(result);
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        /// <summary> Lấy danh sách tất cả sản phẩm </summary>
        [HttpGet]
        [EndpointSummary("Get All Product")]
        [EndpointDescription("Lấy danh sách tất cả sản phẩm.")]
        public async Task<ActionResult<APIResponse>> GetAllProduct()
        {
            try
            {
                var list = await _productService.GetAllProductAsync(GetCancellationToken());
                return SuccessResponse(list);
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        /// <summary> Lấy danh sách tất cả sản phẩm theo category ID </summary>
        [HttpGet("category/{id}")]
        [EndpointSummary("Get All Product By Category")]
        [EndpointDescription("Lấy danh sách tất cả sản phẩm theo category ID.")]
        public async Task<ActionResult<APIResponse>> GetAllProductByCategory([FromRoute] ulong id)
        {
            try
            {
                var list = await _productService.GetAllProductByCategoryIdAsync(id, GetCancellationToken());
                return SuccessResponse(list);
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        /// <summary>
        /// Cập nhật sản phẩm. Nếu gửi kèm ảnh (form-data field: images) thì:
        ///  - Xoá ảnh cũ (Cloudinary + DB)
        ///  - Thêm ảnh mới vào bảng media_links
        /// Nếu KHÔNG gửi ảnh mới -> chỉ cập nhật thông tin, ảnh giữ nguyên.
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [UploadMultipleImages("images", "products")] // middleware upload trước action
        [EndpointSummary("Update Product")]
        [EndpointDescription("Cập nhật thông tin sản phẩm. Nếu có ảnh mới sẽ replace toàn bộ ảnh cũ.")]
        public async Task<ActionResult<APIResponse>> UpdateProduct([FromRoute] ulong id, [FromForm] ProductUpdateDTO dto)
        {
            var validationResult = ValidateModel();
            if (validationResult != null) return validationResult;

            try
            {
                // 1) Update thông tin product (không đụng ảnh)
                var result = await _productService.UpdateProductAsync(id, dto, GetCancellationToken());

                // 2) Ảnh mới sau khi UploadMultipleImages chạy xong?
                var ups = HttpContext.Items["uploadedImages"] as List<UploadResultDto>;
                if (ups != null && ups.Count > 0)
                {
                    // 2a) Lấy danh sách publicId ảnh cũ trong DB
                    var oldPublicIds = await _db.MediaLinks
                        .Where(m => m.OwnerType == MediaOwnerType.Product && m.OwnerId == id)
                        .Select(m => m.ImagePublicId)
                        .Where(pid => pid != null && pid != "")
                        .ToListAsync(GetCancellationToken());

                    // 2b) Xoá file cũ trên Cloudinary
                    foreach (var pid in oldPublicIds)
                        await _cloud.DeleteAsync(pid!, GetCancellationToken());

                    // 2c) Replace DB bằng ảnh mới
                    var dtoImgs = ups.Select((u, i) => new MediaUploadDTO
                    {
                        Url = u.Url,
                        PublicId = u.PublicId,
                        SortOrder = i
                    }).ToList();

                    await _productService.ReplaceProductImagesAsync(id, dtoImgs, GetCancellationToken());
                }

                return SuccessResponse(result);
            }
            catch (KeyNotFoundException)
            {
                return ErrorResponse("Không tìm thấy sản phẩm", HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Thêm ảnh cho sản phẩm (không xoá ảnh cũ). Form-data field: images[]
        /// </summary>
        [HttpPost("{id}/images")]
        [Consumes("multipart/form-data")]
        [UploadMultipleImages("images", "products")]
        [EndpointSummary("Add images to product (append)")]
        public async Task<ActionResult<APIResponse>> AddImages([FromRoute] ulong id)
        {
            try
            {
                var ups = HttpContext.Items["uploadedImages"] as List<UploadResultDto>;
                if (ups == null || ups.Count == 0)
                    return ErrorResponse("Không có ảnh nào được upload.", HttpStatusCode.BadRequest);

                var dtoImgs = ups.Select((u, i) => new MediaUploadDTO
                {
                    Url = u.Url,
                    PublicId = u.PublicId,
                    SortOrder = i
                }).ToList();

                await _productService.AddProductImagesAsync(id, dtoImgs, GetCancellationToken());
                return SuccessResponse(new { linked = dtoImgs.Count });
            }
            catch (KeyNotFoundException)
            {
                return ErrorResponse("Không tìm thấy sản phẩm", HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
