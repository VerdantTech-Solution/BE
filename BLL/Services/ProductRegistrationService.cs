using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.DTO.Media;
using BLL.DTO.MediaLink;
using BLL.DTO.ProductRegistration;
using BLL.Interfaces;
using DAL.Data;
using DAL.Data.Models;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ProductRegistrationService : IProductRegistrationService
    {
        private readonly IProductRegistrationRepository _repo;
        private readonly IMapper _mapper;
        private readonly VerdantTechDbContext _db;

        public ProductRegistrationService(
            IProductRegistrationRepository repo,
            IMapper mapper,
            VerdantTechDbContext db)
        {
            _repo = repo;
            _mapper = mapper;
            _db = db;
        }

        // ================= READS =================

        public async Task<PagedResponse<ProductRegistrationReponseDTO>> GetAllAsync(
            int page, int pageSize, CancellationToken ct = default)
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, ct);
            var dtos = _mapper.Map<List<ProductRegistrationReponseDTO>>(items);
            await HydrateMediaAsync(dtos, ct);
            return ToPaged(dtos, total, page, pageSize);
        }

        public async Task<PagedResponse<ProductRegistrationReponseDTO>> GetByVendorAsync(
            ulong vendorId, int page, int pageSize, CancellationToken ct = default)
        {
            var (items, total) = await _repo.GetByVendorPagedAsync(vendorId, page, pageSize, ct);
            var dtos = _mapper.Map<List<ProductRegistrationReponseDTO>>(items);
            await HydrateMediaAsync(dtos, ct);
            return ToPaged(dtos, total, page, pageSize);
        }

        public async Task<ProductRegistrationReponseDTO?> GetByIdAsync(
            ulong id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, true, ct);
            if (entity is null) return null;

            var dto = _mapper.Map<ProductRegistrationReponseDTO>(entity);
            await HydrateMediaAsync(new List<ProductRegistrationReponseDTO> { dto }, ct);
            return dto;
        }

        // ================ CREATE (có ảnh & file) ================
        public async Task<ProductRegistrationReponseDTO> CreateAsync( ProductRegistrationCreateDTO dto, string? manualUrl, string? manualPublicUrl, IReadOnlyList<MediaLinkItemDTO> images, CancellationToken ct = default)
        {
            // validate FK
            if (!await _db.Users.AnyAsync(x => x.Id == dto.VendorId, ct))
                throw new InvalidOperationException("Vendor không tồn tại.");
            if (!await _db.ProductCategories.AnyAsync(x => x.Id == dto.CategoryId, ct))
                throw new InvalidOperationException("Category không tồn tại.");

            var entity = _mapper.Map<ProductRegistration>(dto);
            entity.Status = ProductRegistrationStatus.Pending;
            entity.ManualUrls = manualUrl;
            entity.PublicUrl = manualPublicUrl;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            entity = await _repo.CreateAsync(entity, ct);

            // lưu ảnh
            await SaveMediaLinksAsync(entity.Id, images, ct);

            var result = _mapper.Map<ProductRegistrationReponseDTO>(entity);
            await HydrateMediaAsync(new List<ProductRegistrationReponseDTO> { result }, ct);
            return result;
        }

        // ================ UPDATE (có thêm bớt ảnh & file) ================
        public async Task<ProductRegistrationReponseDTO> UpdateAsync( ProductRegistrationUpdateDTO dto, string? manualUrl, string? manualPublicUrl, IReadOnlyList<MediaLinkItemDTO> addImages, IReadOnlyList<string> removeImagePublicIds, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(dto.Id, false, ct)
                         ?? throw new KeyNotFoundException("Đơn đăng ký không tồn tại.");

            // update fields
            entity.VendorId = dto.VendorId;
            entity.CategoryId = dto.CategoryId;
            entity.ProposedProductCode = dto.ProposedProductCode;
            entity.ProposedProductName = dto.ProposedProductName;
            entity.Description = dto.Description;
            entity.UnitPrice = dto.UnitPrice;
            entity.EnergyEfficiencyRating = dto.EnergyEfficiencyRating;
            entity.Specifications = dto.Specifications ?? new Dictionary<string, object>();
            entity.DimensionsCm = dto.DimensionsCm ?? new Dictionary<string, decimal>();
            if (!string.IsNullOrWhiteSpace(manualUrl)) entity.ManualUrls = manualUrl;
            if (!string.IsNullOrWhiteSpace(manualPublicUrl)) entity.PublicUrl = manualPublicUrl;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity, ct);

            // xoá ảnh theo public id (nếu có)
            if (removeImagePublicIds is { Count: > 0 })
            {
                var toRemove = await _db.MediaLinks
                    .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration
                             && m.OwnerId == entity.Id
                             && m.ImagePublicId != null
                             && removeImagePublicIds.Contains(m.ImagePublicId))
                    .ToListAsync(ct);

                if (toRemove.Count > 0)
                {
                    _db.MediaLinks.RemoveRange(toRemove);
                    await _db.SaveChangesAsync(ct);
                }
            }

            // thêm ảnh mới (nếu có)
            if (addImages is { Count: > 0 })
            {
                await SaveMediaLinksAsync(entity.Id, addImages, ct);
            }

            var result = _mapper.Map<ProductRegistrationReponseDTO>(entity);
            await HydrateMediaAsync(new List<ProductRegistrationReponseDTO> { result }, ct);
            return result;
        }

        public Task<bool> DeleteAsync(ulong id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);

        public async Task<bool> ChangeStatusAsync(
            ulong id, string status, string? reason, ulong? approvedBy, CancellationToken ct = default)
        {
            if (!Enum.TryParse<ProductRegistrationStatus>(status, true, out var st))
                throw new ArgumentException("Trạng thái không hợp lệ (Pending|Approved|Rejected).");

            return await _repo.ChangeStatusAsync(id, st, reason, approvedBy, ct);
        }

        // ================= Helpers =================
        private async Task SaveMediaLinksAsync( ulong ownerId, IReadOnlyList<MediaLinkItemDTO> images, CancellationToken ct)
        {
            if (images == null || images.Count == 0) return;

            var start = await _db.MediaLinks
                .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration && m.OwnerId == ownerId)
                .Select(m => (int?)m.SortOrder)
                .MaxAsync(ct) ?? 0;

            var now = DateTime.UtcNow;
            var list = images.Select((img, i) => new MediaLink
            {
                OwnerType = MediaOwnerType.ProductRegistration,
                OwnerId = ownerId,
                ImageUrl = img.ImageUrl,       // <— dùng thuộc tính của MediaLinkItemDTO
                ImagePublicId = img.ImagePublicId,  // <— dùng thuộc tính của MediaLinkItemDTO
                Purpose = MediaPurpose.None,
                SortOrder = start + i + 1,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList();

            _db.MediaLinks.AddRange(list);
            await _db.SaveChangesAsync(ct);
        }

        private async Task HydrateMediaAsync( IReadOnlyList<ProductRegistrationReponseDTO> items, CancellationToken ct)
        {
            if (items.Count == 0) return;
            var ids = items.Select(x => x.Id).ToList();

            var medias = await _db.MediaLinks.AsNoTracking()
                .Where(m => m.OwnerType == MediaOwnerType.ProductRegistration && ids.Contains(m.OwnerId))
                .OrderBy(m => m.OwnerId).ThenBy(m => m.SortOrder)
                .ToListAsync(ct);

            var map = medias.GroupBy(m => m.OwnerId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var dto in items)
            {
                if (!map.TryGetValue(dto.Id, out var list)) continue;

                dto.Images = list.Select(m => new MediaLinkItemDTO
                {
                    Id = m.Id,
                    ImagePublicId = m.ImagePublicId,
                    ImageUrl = m.ImageUrl,
                    Purpose = m.Purpose.ToString().ToLowerInvariant(),
                    SortOrder = m.SortOrder
                }).ToList();
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
