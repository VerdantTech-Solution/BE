using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.DTO.MediaLink;
using BLL.DTO.ProductRegistration;
using BLL.Interfaces;
using DAL.Data;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ProductRegistrationService : IProductRegistrationService
    {
        private readonly VerdantTechDbContext _db;
        private readonly IMapper _mapper;

        public ProductRegistrationService(VerdantTechDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // CREATE
        public async Task<ProductRegistrationReponseDTO> CreateAsync(ProductRegistrationCreateDTO dto, CancellationToken ct = default)
        {
            if (!await _db.Users.AnyAsync(x => x.Id == dto.VendorId, ct))
                throw new InvalidOperationException("Vendor không tồn tại.");
            if (!await _db.ProductCategories.AnyAsync(x => x.Id == dto.CategoryId, ct))
                throw new InvalidOperationException("Category không tồn tại.");

            var entity = _mapper.Map<ProductRegistration>(dto);
            entity.Status = ProductRegistrationStatus.Pending;
            entity.ManualUrls = dto.ManualUrl;
            //entity.PublicUrl = dto.ManualPublicUrl;
            entity.Specifications = dto.Specifications ?? new Dictionary<string, object>();
            entity.DimensionsCm = dto.DimensionsCm != null
                ? new Dictionary<string, decimal>
                  {
                      ["length"] = dto.DimensionsCm.Length,
                      ["width"]  = dto.DimensionsCm.Width,
                      ["height"] = dto.DimensionsCm.Height
                  }
                : new Dictionary<string, decimal>();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            _db.ProductRegistrations.Add(entity);
            await _db.SaveChangesAsync(ct);

            await SaveProductImagesAsync(entity.Id, dto.ProductImages, ct);
            await SaveCertificatesAsync(entity.Id, dto.CertificateFiles, ct);

            var res = _mapper.Map<ProductRegistrationReponseDTO>(entity);
            await HydrateMediaAsync(new List<ProductRegistrationReponseDTO> { res }, ct);
            return res;
        }

        // READS
        public async Task<PagedResponse<ProductRegistrationReponseDTO>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var q = _db.ProductRegistrations.AsNoTracking().OrderByDescending(x => x.CreatedAt);
            var total = await q.CountAsync(ct);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            var dtos = _mapper.Map<List<ProductRegistrationReponseDTO>>(items);
            await HydrateMediaAsync(dtos, ct);
            return ToPaged(dtos, total, page, pageSize);
        }

        public async Task<PagedResponse<ProductRegistrationReponseDTO>> GetByVendorAsync(ulong vendorId, int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var q = _db.ProductRegistrations.AsNoTracking()
                    .Where(x => x.VendorId == vendorId)
                    .OrderByDescending(x => x.CreatedAt);

            var total = await q.CountAsync(ct);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            var dtos = _mapper.Map<List<ProductRegistrationReponseDTO>>(items);
            await HydrateMediaAsync(dtos, ct);
            return ToPaged(dtos, total, page, pageSize);
        }

        public async Task<ProductRegistrationReponseDTO?> GetByIdAsync(ulong id, CancellationToken ct = default)
        {
            var entity = await _db.ProductRegistrations.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return null;

            var dto = _mapper.Map<ProductRegistrationReponseDTO>(entity);
            await HydrateMediaAsync(new List<ProductRegistrationReponseDTO> { dto }, ct);
            return dto;
        }

        // UPDATE
        public async Task<ProductRegistrationReponseDTO> UpdateAsync(ProductRegistrationUpdateDTO dto, CancellationToken ct = default)
        {
            var entity = await _db.ProductRegistrations.FirstOrDefaultAsync(x => x.Id == dto.Id, ct)
                         ?? throw new KeyNotFoundException("Đơn đăng ký không tồn tại.");

            entity.VendorId = dto.VendorId;
            entity.CategoryId = dto.CategoryId;
            entity.ProposedProductCode = dto.ProposedProductCode;
            entity.ProposedProductName = dto.ProposedProductName;
            entity.Description = dto.Description;
            entity.UnitPrice = dto.UnitPrice;
            entity.EnergyEfficiencyRating = dto.EnergyEfficiencyRating;
            entity.Specifications = dto.Specifications ?? new Dictionary<string, object>();
            entity.DimensionsCm = dto.DimensionsCm != null
                 ? new Dictionary<string, decimal>
                 {
                     ["length"] = dto.DimensionsCm.Length,
                     ["width"] = dto.DimensionsCm.Width,
                     ["height"] = dto.DimensionsCm.Height
                 }
                 : new Dictionary<string, decimal>();
            if (!string.IsNullOrWhiteSpace(dto.ManualUrl)) entity.ManualUrls = dto.ManualUrl;
            if (!string.IsNullOrWhiteSpace(dto.ManualPublicUrl)) entity.PublicUrl = dto.ManualPublicUrl;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            // remove images
            if (dto.RemoveImagePublicIds is { Count: > 0 })
            {
                var toRemove = await _db.MediaLinks
                    .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration
                             && m.OwnerId == entity.Id
                             && m.ImagePublicId != null
                             && dto.RemoveImagePublicIds.Contains(m.ImagePublicId))
                    .ToListAsync(ct);

                if (toRemove.Count > 0)
                {
                    _db.MediaLinks.RemoveRange(toRemove);
                    await _db.SaveChangesAsync(ct);
                }
            }

            // add images
            await SaveProductImagesAsync(entity.Id, dto.AddProductImages, ct);

            // remove certificate medias
            if (dto.RemoveCertificatePublicIds is { Count: > 0 })
            {
                var rmCertMedia = await _db.MediaLinks
                    .Where(m => m.OwnerType == MediaOwnerType.ProductCertificate
                             && m.OwnerId == entity.Id
                             && m.ImagePublicId != null
                             && dto.RemoveCertificatePublicIds.Contains(m.ImagePublicId))
                    .ToListAsync(ct);

                if (rmCertMedia.Count > 0)
                {
                    _db.MediaLinks.RemoveRange(rmCertMedia);
                    await _db.SaveChangesAsync(ct);
                }

                // (Nếu cần xoá row product_certificates, bật block dưới và map đúng tên property)
                // var rmCertRows = await _db.ProductCertificates
                //     .Where(c => c.ProductRegistrationId == entity.Id
                //              && c.FilePublicId != null
                //              && dto.RemoveCertificatePublicIds.Contains(c.FilePublicId))
                //     .ToListAsync(ct);
                // if (rmCertRows.Count > 0) { _db.ProductCertificates.RemoveRange(rmCertRows); await _db.SaveChangesAsync(ct); }
            }

            // add certificates
            await SaveCertificatesAsync(entity.Id, dto.AddCertificateFiles, ct);

            var res = _mapper.Map<ProductRegistrationReponseDTO>(entity);
            await HydrateMediaAsync(new List<ProductRegistrationReponseDTO> { res }, ct);
            return res;
        }

        // CHANGE STATUS
        public async Task<bool> ChangeStatusAsync(ulong id, ProductRegistrationStatus status, string? rejectionReason, ulong? approvedBy, CancellationToken ct = default)
        {
            var entity = await _db.ProductRegistrations.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return false;

            entity.Status = status;
            entity.RejectionReason = status == ProductRegistrationStatus.Rejected ? rejectionReason : null;

            if (status == ProductRegistrationStatus.Approved)
            {
                entity.ApprovedBy = approvedBy;
                entity.ApprovedAt = DateTime.UtcNow;

                // Auto create Product
                var product = new Product
                {
                    VendorId = entity.VendorId,
                    CategoryId = entity.CategoryId,
                    ProductCode = entity.ProposedProductCode,
                    ProductName = entity.ProposedProductName,   // dùng ProductName để tránh lỗi 'Name'
                    Description = entity.Description,
                    UnitPrice = entity.UnitPrice,
                    EnergyEfficiencyRating = entity.EnergyEfficiencyRating,
                    Specifications = entity.Specifications ?? new Dictionary<string, object>(),
                    DimensionsCm = entity.DimensionsCm ?? new Dictionary<string, decimal>(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                    // KHÔNG set Status để tránh lỗi nếu model không có field này
                };
                _db.Products.Add(product);
                await _db.SaveChangesAsync(ct);

                // Copy ảnh từ Registration sang Product
                var regImages = await _db.MediaLinks
                    .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration && m.OwnerId == entity.Id)
                    .OrderBy(m => m.SortOrder)
                    .ToListAsync(ct);

                if (regImages.Count > 0)
                {
                    var now = DateTime.UtcNow;
                    var rows = regImages.Select(x => new MediaLink
                    {
                        OwnerType = MediaOwnerType.Product,
                        OwnerId = product.Id,
                        ImageUrl = x.ImageUrl,
                        ImagePublicId = x.ImagePublicId,
                        Purpose = x.Purpose,
                        SortOrder = x.SortOrder,
                        CreatedAt = now,
                        UpdatedAt = now
                    }).ToList();

                    _db.MediaLinks.AddRange(rows);
                    await _db.SaveChangesAsync(ct);
                }
            }

            entity.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(ulong id, CancellationToken ct = default)
        {
            var entity = await _db.ProductRegistrations.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) return false;

            var medias = await _db.MediaLinks
                .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration && m.OwnerId == id)
                .ToListAsync(ct);
            if (medias.Count > 0) _db.MediaLinks.RemoveRange(medias);

            var certMedias = await _db.MediaLinks
                .Where(m => m.OwnerType == MediaOwnerType.ProductCertificate && m.OwnerId == id)
                .ToListAsync(ct);
            if (certMedias.Count > 0) _db.MediaLinks.RemoveRange(certMedias);

            // (Nếu cần xóa row product_certificates, bật block dưới và map đúng tên property)
            // var certRows = await _db.ProductCertificates.Where(c => c.ProductRegistrationId == id).ToListAsync(ct);
            // if (certRows.Count > 0) _db.ProductCertificates.RemoveRange(certRows);

            _db.ProductRegistrations.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // Helpers
        private async Task SaveProductImagesAsync(ulong ownerId, IReadOnlyList<MediaLinkItemDTO>? images, CancellationToken ct)
        {
            if (images == null || images.Count == 0) return;

            var now = DateTime.UtcNow;
            var baseSort = await _db.MediaLinks
                .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration && m.OwnerId == ownerId)
                .Select(m => (int?)m.SortOrder).MaxAsync(ct) ?? 0;

            var rows = images.Select((img, i) => new MediaLink
            {
                OwnerType = MediaOwnerType.ProductRegistration,
                OwnerId = ownerId,
                ImageUrl = img.ImageUrl,
                ImagePublicId = img.ImagePublicId,
                Purpose = ParsePurpose(img.Purpose),
                SortOrder = img.SortOrder > 0 ? img.SortOrder : baseSort + i + 1,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList();

            _db.MediaLinks.AddRange(rows);
            await _db.SaveChangesAsync(ct);
        }

        private async Task SaveCertificatesAsync(ulong registrationId, IReadOnlyList<MediaLinkItemDTO>? files, CancellationToken ct)
        {
            if (files == null || files.Count == 0) return;

            var now = DateTime.UtcNow;
            var baseSort = await _db.MediaLinks
                .Where(m => m.OwnerType == MediaOwnerType.ProductCertificate && m.OwnerId == registrationId)
                .Select(m => (int?)m.SortOrder).MaxAsync(ct) ?? 0;

            var mediaRows = files.Select((f, i) => new MediaLink
            {
                OwnerType = MediaOwnerType.ProductCertificate,
                OwnerId = registrationId,
                ImageUrl = f.ImageUrl,         // link file PDF certificate
                ImagePublicId = f.ImagePublicId,
                Purpose = MediaPurpose.None,
                SortOrder = f.SortOrder > 0 ? f.SortOrder : baseSort + i + 1,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList();

            _db.MediaLinks.AddRange(mediaRows);

            // Nếu model ProductCertificate của bạn có property tên khác,
            // var certRows = files.Select(f => new ProductCertificate
            // {
            //     ProductRegistrationId = registrationId,
            //     CertificateType = "file",
            //     FileUrl = f.ImageUrl,
            //     FilePublicId = f.ImagePublicId,
            //     CreatedAt = now,
            //     UpdatedAt = now
            // }).ToList();
            // _db.ProductCertificates.AddRange(certRows);

            await _db.SaveChangesAsync(ct);
        }

        private static MediaPurpose ParsePurpose(string? p) =>
            p?.ToLowerInvariant() switch
            {
                "front" => MediaPurpose.Front,
                "back" => MediaPurpose.Back,
                _ => MediaPurpose.None
            };

        private async Task HydrateMediaAsync(IReadOnlyList<ProductRegistrationReponseDTO> items, CancellationToken ct)
        {
            if (items.Count == 0) return;
            var ids = items.Select(x => x.Id).ToList();

            var imgs = await _db.MediaLinks.AsNoTracking()
                .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration && ids.Contains(m.OwnerId))
                .OrderBy(m => m.OwnerId).ThenBy(m => m.SortOrder)
                .ToListAsync(ct);

            var certs = await _db.MediaLinks.AsNoTracking()
                .Where(m => m.OwnerType == MediaOwnerType.ProductCertificate && ids.Contains(m.OwnerId))
                .OrderBy(m => m.OwnerId).ThenBy(m => m.SortOrder)
                .ToListAsync(ct);

            var imgMap = imgs.GroupBy(m => m.OwnerId).ToDictionary(g => g.Key, g => g.ToList());
            var certMap = certs.GroupBy(m => m.OwnerId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var dto in items)
            {
                if (imgMap.TryGetValue(dto.Id, out var list))
                {
                    dto.Images = list.Select(m => new MediaLinkItemDTO
                    {
                        Id = m.Id,
                        ImagePublicId = m.ImagePublicId,
                        ImageUrl = m.ImageUrl,
                        Purpose = m.Purpose.ToString().ToLowerInvariant(),
                        SortOrder = m.SortOrder
                    }).ToList();
                }

                if (certMap.TryGetValue(dto.Id, out var clist))
                {
                    dto.Certificates = clist.Select(m => new MediaLinkItemDTO
                    {
                        Id = m.Id,
                        ImagePublicId = m.ImagePublicId,
                        ImageUrl = m.ImageUrl,
                        Purpose = m.Purpose.ToString().ToLowerInvariant(),
                        SortOrder = m.SortOrder
                    }).ToList();
                }

                // map manual
                dto.ManualUrl = dto.ManualUrl ?? items
                    .Where(x => x.Id == dto.Id).Select(x => x.ManualUrl).FirstOrDefault();
                dto.ManualPublicUrl = dto.ManualPublicUrl ?? items
                    .Where(x => x.Id == dto.Id).Select(x => x.ManualPublicUrl).FirstOrDefault();
            }
        }

        private static PagedResponse<T> ToPaged<T>(List<T> items, int totalRecords, int page, int pageSize)
        {
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            return new PagedResponse<T>
            {
                Data = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }
    }
}
