using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using BLL.DTO;
using BLL.DTO.MediaLink;
using BLL.DTO.ProductRegistration;
using BLL.Interfaces;
using Infrastructure.Cloudinary;
using BLL.DTO.Product; // cần cho ProductUpdateDTO.DimensionsDTO

namespace Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductRegistrationController : ControllerBase
    {
        private readonly IProductRegistrationService _service;
        private readonly ICloudinaryService _cloud;

        public ProductRegistrationController(IProductRegistrationService service, ICloudinaryService cloud)
        {
            _service = service;
            _cloud = cloud;
        }

        // ===== READS =====

        [HttpGet]
        [EndpointSummary("Danh sách đăng ký sản phẩm (phân trang)")]
        [EndpointDescription("Trả về danh sách ProductRegistration kèm đầy đủ media (Images, Certificates) và ManualUrl/ManualPublicUrl.")]
        public async Task<ActionResult<PagedResponse<ProductRegistrationReponseDTO>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
            => Ok(await _service.GetAllAsync(page, pageSize, ct));

        [HttpGet("{id:long}")]
        [EndpointSummary("Lấy chi tiết đăng ký theo Id")]
        [EndpointDescription("Bao gồm metadata, ảnh, chứng chỉ, ManualUrl/ManualPublicUrl.")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> GetById(long id, CancellationToken ct = default)
        {
            var item = await _service.GetByIdAsync((ulong)id, ct);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("vendor/{vendorId:long}")]
        [EndpointSummary("Lấy các đăng ký theo Vendor (phân trang)")]
        [EndpointDescription("Lọc theo VendorId, trả về media và manual đầy đủ.")]
        public async Task<ActionResult<PagedResponse<ProductRegistrationReponseDTO>>> GetByVendor(
            long vendorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => Ok(await _service.GetByVendorAsync((ulong)vendorId, page, pageSize, ct));

        // ===== CREATE (multipart/form-data) =====

        [HttpPost]
        [Consumes("multipart/form-data")]
        [EndpointSummary("Tạo đăng ký sản phẩm")]
        [EndpointDescription("Form-data: Data = ProductRegistrationCreateDTO (các field Data.*), ManualFile (PDF), Images[] (ảnh), CertificateFiles[] (chứng chỉ).")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> Create(
            [FromForm] CreateFormRequest req,
            CancellationToken ct = default)
        {
            // Map về DTO “chuẩn” để đảm bảo đúng kiểu
            var dto = new ProductRegistrationCreateDTO
            {
                VendorId = req.Data.VendorId,
                CategoryId = req.Data.CategoryId,
                ProposedProductCode = req.Data.ProposedProductCode,
                ProposedProductName = req.Data.ProposedProductName,
                Description = req.Data.Description,
                UnitPrice = req.Data.UnitPrice,
                // int? -> string (service/DB đang dùng string)
                EnergyEfficiencyRating = req.Data.EnergyEfficiencyRating?.ToString(),
                Specifications = req.Data.Specifications,
                WarrantyMonths = req.Data.WarrantyMonths,
                WeightKg = req.Data.WeightKg,
                // Dictionary -> DimensionsDTO
                DimensionsCm = new ProductUpdateDTO.DimensionsDTO
                {
                    Width = req.Data.DimensionsCmWidth ?? 0,
                    Height = req.Data.DimensionsCmHeight ?? 0,
                    Length = req.Data.DimensionsCmLength ?? 0
                }
            };

            // Manual PDF (tùy chọn)
            if (req.ManualFile is not null)
            {
                var up = await _cloud.UploadAsync(req.ManualFile, "product-registrations/manuals", ct);
                dto.ManualUrl = up.Url;
                dto.ManualPublicUrl = up.PublicUrl;
            }

            // Ảnh sản phẩm (tùy chọn)
            if (req.Images is { Count: > 0 })
            {
                var ups = await _cloud.UploadManyAsync(req.Images, "product-registrations/images", ct);
                dto.ProductImages = ups.Select((x, i) => new MediaLinkItemDTO
                {
                    ImagePublicId = x.PublicUrl,
                    ImageUrl = x.Url,
                    Purpose = "none",
                    SortOrder = i + 1
                }).ToList();
            }

            // File chứng chỉ (tùy chọn)
            if (req.CertificateFiles is { Count: > 0 })
            {
                var ups = await _cloud.UploadManyAsync(req.CertificateFiles, "product-registrations/certificates", ct);
                dto.CertificateFiles = ups.Select((x, i) => new MediaLinkItemDTO
                {
                    ImagePublicId = x.PublicUrl,
                    ImageUrl = x.Url,
                    Purpose = "none",
                    SortOrder = i + 1
                }).ToList();
            }

            var created = await _service.CreateAsync(dto, ct);
            return Ok(created);
        }

        // ===== UPDATE (multipart/form-data) =====

        [HttpPut("{id:long}")]
        [Consumes("multipart/form-data")]
        [EndpointSummary("Cập nhật đăng ký sản phẩm")]
        [EndpointDescription("Form-data: Data = ProductRegistrationUpdateDTO (bắt buộc có Id), ManualFile (PDF – tùy chọn), Images[] (ảnh mới cần thêm), CertificateFiles[] (chứng chỉ mới cần thêm), RemoveImagePublicIds[] (ảnh cần xoá), RemoveCertificatePublicIds[] (chứng chỉ cần xoá).")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> Update(
            long id,
            [FromForm] UpdateFormRequest req,
            CancellationToken ct = default)
        {
            if ((ulong)id != req.Data.Id) return BadRequest("Id không khớp.");

            var dto = new ProductRegistrationUpdateDTO
            {
                Id = req.Data.Id,
                VendorId = req.Data.VendorId,
                CategoryId = req.Data.CategoryId,
                ProposedProductCode = req.Data.ProposedProductCode,
                ProposedProductName = req.Data.ProposedProductName,
                Description = req.Data.Description,
                UnitPrice = req.Data.UnitPrice,
                EnergyEfficiencyRating = req.Data.EnergyEfficiencyRating?.ToString(), // int? -> string
                Specifications = req.Data.Specifications,
                WarrantyMonths = req.Data.WarrantyMonths,
                WeightKg = req.Data.WeightKg,
                DimensionsCm = new ProductUpdateDTO.DimensionsDTO
                {
                    Width = req.Data.DimensionsCmWidth ?? 0,
                    Height = req.Data.DimensionsCmHeight ?? 0,
                    Length = req.Data.DimensionsCmLength ?? 0
                },
                RemoveImagePublicIds = req.RemoveImagePublicIds ?? new List<string>(),
                RemoveCertificatePublicIds = req.RemoveCertificatePublicIds ?? new List<string>()
            };

            // Manual mới (nếu có)
            if (req.ManualFile is not null)
            {
                var up = await _cloud.UploadAsync(req.ManualFile, "product-registrations/manuals", ct);
                dto.ManualUrl = up.Url;
                dto.ManualPublicUrl = up.PublicUrl;
            }

            // Ảnh mới cần thêm
            if (req.Images is { Count: > 0 })
            {
                var ups = await _cloud.UploadManyAsync(req.Images, "product-registrations/images", ct);
                dto.AddProductImages = ups.Select((x, i) => new MediaLinkItemDTO
                {
                    ImagePublicId = x.PublicUrl,
                    ImageUrl = x.Url,
                    Purpose = "none",
                    SortOrder = i + 1
                }).ToList();
            }

            // Chứng chỉ mới cần thêm
            if (req.CertificateFiles is { Count: > 0 })
            {
                var ups = await _cloud.UploadManyAsync(req.CertificateFiles, "product-registrations/certificates", ct);
                dto.AddCertificateFiles = ups.Select((x, i) => new MediaLinkItemDTO
                {
                    ImagePublicId = x.PublicUrl,
                    ImageUrl = x.Url,
                    Purpose = "none",
                    SortOrder = i + 1
                }).ToList();
            }

            var updated = await _service.UpdateAsync(dto, ct);
            return Ok(updated);
        }

        // ===== CHANGE STATUS / DELETE =====

        [HttpPatch("{id:long}/status")]
        [EndpointSummary("Duyệt / từ chối đơn đăng ký")]
        [EndpointDescription("Đổi trạng thái Pending | Approved | Rejected. Nếu Approved sẽ auto tạo Product và copy ảnh sang Product.")]
        public async Task<IActionResult> ChangeStatus(
            long id,
            [FromBody] ProductRegistrationChangeStatusDTO dto,
            CancellationToken ct = default)
        {
            var ok = await _service.ChangeStatusAsync((ulong)id, dto.Status, dto.RejectionReason, dto.ApprovedBy, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:long}")]
        [EndpointSummary("Xóa đơn đăng ký sản phẩm")]
        [EndpointDescription("Xóa đơn đăng ký và dọn media links (ảnh + certificate).")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct = default)
            => (await _service.DeleteAsync((ulong)id, ct)) ? NoContent() : NotFound();

        // ===== Request models: gom field upload + Data là DTO sẵn có =====
        public sealed class CreateFormRequest
        {
            [FromForm] public ProductRegistrationCreateDTO Data { get; set; } = null!;
            [FromForm] public IFormFile? ManualFile { get; set; }
            [FromForm] public List<IFormFile>? Images { get; set; }
            [FromForm] public List<IFormFile>? CertificateFiles { get; set; }
        }

        public sealed class UpdateFormRequest
        {
            [FromForm] public ProductRegistrationUpdateDTO Data { get; set; } = null!;
            [FromForm] public IFormFile? ManualFile { get; set; }
            [FromForm] public List<IFormFile>? Images { get; set; }
            [FromForm] public List<IFormFile>? CertificateFiles { get; set; }
            [FromForm] public List<string>? RemoveImagePublicIds { get; set; }
            [FromForm] public List<string>? RemoveCertificatePublicIds { get; set; }
        }
    }
}
