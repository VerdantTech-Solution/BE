using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using BLL.DTO;
using BLL.DTO.ProductRegistration;
using BLL.DTO.MediaLink;
using BLL.Interfaces;
using Infrastructure.Cloudinary; // đổi theo namespace service upload thực tế
using BLL.DTO.Product; // cần cho ProductUpdateDTO.DimensionsDTO

namespace Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductRegistrationController : ControllerBase
    {
        private readonly IProductRegistrationService _service;
        private readonly ICloudinaryService _cloud; // service upload của bạn trả về Url & PublicId

        public ProductRegistrationController(
            IProductRegistrationService service,
            ICloudinaryService cloud)
        {
            _service = service;
            _cloud = cloud;
        }

        // ========= READ =========
        [HttpGet]
        public async Task<ActionResult<PagedResponse<ProductRegistrationReponseDTO>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => Ok(await _service.GetAllAsync(page, pageSize, ct));

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> GetById(long id, CancellationToken ct = default)
        {
            var item = await _service.GetByIdAsync((ulong)id, ct);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("vendor/{vendorId:long}")]
        public async Task<ActionResult<PagedResponse<ProductRegistrationReponseDTO>>> GetByVendor(
            long vendorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => Ok(await _service.GetByVendorAsync((ulong)vendorId, page, pageSize, ct));

        // ========= CREATE / UPDATE (JSON, dùng DTO đã có) =========
        [HttpPost]
        [Authorize(Roles = "Admin,Staff,Vendor")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> CreateJson(
            [FromBody] CreateRequest req, CancellationToken ct = default)
        {
            req.Images ??= new();
            var created = await _service.CreateAsync(
                req.Data, req.ManualUrl, req.ManualPublicUrl, req.Images, ct);
            return Ok(created);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin,Staff,Vendor")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> UpdateJson(
            long id, [FromBody] UpdateRequest req, CancellationToken ct = default)
        {
            if ((ulong)id != req.Data.Id) return BadRequest("Id không khớp.");
            req.AddImages ??= new();
            req.RemoveImagePublicIds ??= new();

            var updated = await _service.UpdateAsync(
                req.Data, req.ManualUrl, req.ManualPublicUrl, req.AddImages, req.RemoveImagePublicIds, ct);
            return Ok(updated);
        }

        // ========= CREATE / UPDATE (FORM upload ảnh + PDF) =========
        [HttpPost("form")]
        [Authorize(Roles = "Admin,Staff,Vendor")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> CreateForm(
            [FromForm] CreateFormRequest req, CancellationToken ct = default)
        {
            // upload PDF nếu có
            string? manualUrl = null, manualPublicUrl = null;
            if (req.ManualFile is not null)
            {
                var up = await _cloud.UploadAsync(req.ManualFile, "product-registrations/manuals", ct);
                manualUrl = up.Url;
                manualPublicUrl = up.Url; // public URL
            }

            // upload ảnh nếu có
            var imageDtos = new List<MediaLinkItemDTO>();
            if (req.Images?.Count > 0)
            {
                var ups = await _cloud.UploadManyAsync(req.Images, "product-registrations/images", ct);
                imageDtos = ups.Select(x => new MediaLinkItemDTO
                {
                    ImagePublicId = x.PublicId,
                    ImageUrl = x.Url,
                    SortOrder = 0
                }).ToList();
            }

            var created = await _service.CreateAsync(req.Data, manualUrl, manualPublicUrl, imageDtos, ct);
            return Ok(created);
        }

        [HttpPut("{id:long}/form")]
        [Authorize(Roles = "Admin,Staff,Vendor")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductRegistrationReponseDTO>> UpdateForm(
            long id, [FromForm] UpdateFormRequest req, CancellationToken ct = default)
        {
            if ((ulong)id != req.Data.Id) return BadRequest("Id không khớp.");

            string? manualUrl = null, manualPublicUrl = null;
            if (req.ManualFile is not null)
            {
                var up = await _cloud.UploadAsync(req.ManualFile, "product-registrations/manuals", ct);
                manualUrl = up.Url;
                manualPublicUrl = up.Url;
            }

            var addImages = new List<MediaLinkItemDTO>();
            if (req.NewImages?.Count > 0)
            {
                var ups = await _cloud.UploadManyAsync(req.NewImages, "product-registrations/images", ct);
                addImages = ups.Select(x => new MediaLinkItemDTO
                {
                    ImagePublicId = x.PublicId,
                    ImageUrl = x.Url,
                    SortOrder = 0
                }).ToList();
            }

            var remove = req.RemoveImagePublicIds ?? new List<string>();
            var updated = await _service.UpdateAsync(req.Data, manualUrl, manualPublicUrl, addImages, remove, ct);
            return Ok(updated);
        }

        // ========= STATUS / DELETE =========
        [HttpPatch("{id:long}/status")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ChangeStatus(long id,
            [FromBody] ProductRegistrationChangeStatusDTO dto, CancellationToken ct = default)
        {
            var ok = await _service.ChangeStatusAsync((ulong)id, dto.Status, dto.Reason, dto.ApprovedBy, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct = default)
            => (await _service.DeleteAsync((ulong)id, ct)) ? NoContent() : NotFound();

        // ====== Request wrappers (chỉ cho API, vẫn dùng DTO hiện có) ======
        public sealed class CreateRequest
        {
            public ProductRegistrationCreateDTO Data { get; set; } = null!;
            public string? ManualUrl { get; set; }
            public string? ManualPublicUrl { get; set; }
            public List<MediaLinkItemDTO>? Images { get; set; }
        }

        public sealed class UpdateRequest
        {
            public ProductRegistrationUpdateDTO Data { get; set; } = null!;
            public string? ManualUrl { get; set; }
            public string? ManualPublicUrl { get; set; }
            public List<MediaLinkItemDTO>? AddImages { get; set; }
            public List<string>? RemoveImagePublicIds { get; set; }
        }

        public sealed class CreateFormRequest
        {
            public ProductRegistrationCreateDTO Data { get; set; } = null!;
            public IFormFile? ManualFile { get; set; }
            public List<IFormFile>? Images { get; set; }
        }

        public sealed class UpdateFormRequest
        {
            public ProductRegistrationUpdateDTO Data { get; set; } = null!;
            public IFormFile? ManualFile { get; set; }
            public List<IFormFile>? NewImages { get; set; }
            public List<string>? RemoveImagePublicIds { get; set; }
        }
    }
}
