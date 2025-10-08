using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.DTO.ForumPost;
using BLL.DTO.Media;
using BLL.Interfaces;
using DAL.Data;
using DAL.Data.Models;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ForumPostService : IForumPostService
    {
        private readonly IForumPostRepository _repo;
        private readonly IMapper _mapper;
        private readonly VerdantTechDbContext _db;

        public ForumPostService(IForumPostRepository repo, IMapper mapper, VerdantTechDbContext db)
        {
            _repo = repo;
            _mapper = mapper;
            _db = db;
        }

        // ================= READS =================
        public async Task<PagedResponse<ForumPostResponseDTO>> GetAllAsync(int page, int pageSize, ForumPostStatus? status = null, CancellationToken ct = default)
        {
            var (items, total) = await _repo.GetAllAsync(page, pageSize, status, includeNavigation: true, cancellationToken: ct);
            var dtoItems = _mapper.Map<List<ForumPostResponseDTO>>(items);
            await HydrateMediaAsync(dtoItems, ct);
            return ToPaged(dtoItems, total, page, pageSize);
        }

        public async Task<PagedResponse<ForumPostResponseDTO>> GetByCategoryAsync(ulong categoryId, int page, int pageSize, ForumPostStatus? status = null, CancellationToken ct = default)
        {
            var (items, total) = await _repo.GetByCategoryAsync(categoryId, page, pageSize, status, includeNavigation: true, cancellationToken: ct);
            var dtoItems = _mapper.Map<List<ForumPostResponseDTO>>(items);
            await HydrateMediaAsync(dtoItems, ct);
            return ToPaged(dtoItems, total, page, pageSize);
        }

        public async Task<ForumPostResponseDTO?> GetByIdAsync(ulong id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, includeNavigation: true, cancellationToken: ct);
            if (entity is null) return null;
            var dto = _mapper.Map<ForumPostResponseDTO>(entity);
            await HydrateMediaAsync(new List<ForumPostResponseDTO> { dto }, ct);
            return dto;
        }

        public async Task<ForumPostResponseDTO?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var entity = await _repo.GetBySlugAsync(slug, includeNavigation: true, cancellationToken: ct);
            if (entity is null) return null;
            var dto = _mapper.Map<ForumPostResponseDTO>(entity);
            await HydrateMediaAsync(new List<ForumPostResponseDTO> { dto }, ct);
            return dto;
        }

        // ================ SINGLE CREATE (có media) ================
        public async Task<ForumPostResponseDTO> CreateAsync(ForumPostCreateDTO dto, MediaUploadDTO? cover, IReadOnlyList<MediaUploadDTO> gallery, CancellationToken ct = default)
        {
            // Tránh lỗi FK user_id
            var userExists = await _db.Users.AnyAsync(u => u.Id == dto.UserId, ct);
            if (!userExists) throw new Exception("User không tồn tại.");

            var entity = _mapper.Map<ForumPost>(dto);
            entity.UserId = dto.UserId;

            // Slug an toàn & duy nhất
            var rawSlug = string.IsNullOrWhiteSpace(dto.Slug) ? dto.Title : dto.Slug;
            var desiredSlug = Slugify(rawSlug);
            entity.Slug = await EnsureUniqueSlugAsync(desiredSlug, ignoreId: null, ct);

            entity.ViewCount = 0;
            entity.LikeCount = 0;
            entity.DislikeCount = 0;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var created = await _repo.CreateAsync(entity, ct);

            // Lưu media links (cover + gallery)
            await SaveMediaLinksAsync(created.Id, cover, gallery, ct);

            var result = _mapper.Map<ForumPostResponseDTO>(created);
            await HydrateMediaAsync(new List<ForumPostResponseDTO> { result }, ct);
            return result;
        }

        // ================ SINGLE UPDATE (có media) ================
        public async Task<ForumPostResponseDTO> UpdateAsync(ForumPostUpdateDTO dto, MediaUploadDTO? newCover, IReadOnlyList<MediaUploadDTO> newGallery, IReadOnlyList<string> removePublicIds, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(dto.Id, includeNavigation: false, cancellationToken: ct)
                         ?? throw new Exception("Forum post not found");

            entity.ForumCategoryId = dto.ForumCategoryId;
            entity.Title = dto.Title;
            entity.Content = dto.Content;
            entity.Tags = dto.Tags;
            entity.IsPinned = dto.IsPinned;
            entity.Status = dto.Status;

            // Nếu client đổi slug (hoặc để trống), chuẩn hoá & đảm bảo duy nhất
            var desiredRaw = string.IsNullOrWhiteSpace(dto.Slug) ? dto.Title : dto.Slug;
            var desiredSlug = Slugify(desiredRaw);
            if (!string.Equals(entity.Slug, desiredSlug, StringComparison.OrdinalIgnoreCase))
            {
                entity.Slug = await EnsureUniqueSlugAsync(desiredSlug, ignoreId: dto.Id, ct);
            }

            entity.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(entity, ct);

            // Xoá MediaLink theo publicId (file đã được controller xoá trên Cloudinary)
            if (removePublicIds != null && removePublicIds.Count > 0)
            {
                var needRemove = await _db.MediaLinks
                    .Where(m => m.OwnerType == MediaOwnerType.ForumPost
                             && m.OwnerId == dto.Id
                             && m.ImagePublicId != null
                             && removePublicIds.Contains(m.ImagePublicId))
                    .ToListAsync(ct);

                if (needRemove.Count > 0)
                {
                    _db.MediaLinks.RemoveRange(needRemove);
                    await _db.SaveChangesAsync(ct);
                }
            }

            // Thay cover
            if (newCover != null)
            {
                var oldCover = await _db.MediaLinks
                    .Where(m => m.OwnerType == MediaOwnerType.ForumPost
                             && m.OwnerId == dto.Id
                             && m.Purpose == MediaPurpose.Front)
                    .ToListAsync(ct);

                if (oldCover.Count > 0)
                {
                    _db.MediaLinks.RemoveRange(oldCover);
                    await _db.SaveChangesAsync(ct);
                }

                _db.MediaLinks.Add(new MediaLink
                {
                    OwnerType = MediaOwnerType.ForumPost,
                    OwnerId = dto.Id,
                    ImageUrl = newCover.Url,
                    ImagePublicId = newCover.PublicId,
                    Purpose = MediaPurpose.Front,
                    SortOrder = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                await _db.SaveChangesAsync(ct);
            }

            // Thêm gallery mới (nếu có)
            if (newGallery != null && newGallery.Count > 0)
            {
                var maxSort = await _db.MediaLinks
                    .Where(m => m.OwnerType == MediaOwnerType.ForumPost && m.OwnerId == dto.Id)
                    .Select(m => (int?)m.SortOrder).MaxAsync(ct) ?? 0;

                int i = 1;
                foreach (var g in newGallery)
                {
                    _db.MediaLinks.Add(new MediaLink
                    {
                        OwnerType = MediaOwnerType.ForumPost,
                        OwnerId = dto.Id,
                        ImageUrl = g.Url,
                        ImagePublicId = g.PublicId,
                        Purpose = MediaPurpose.None,
                        SortOrder = maxSort + i++,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                await _db.SaveChangesAsync(ct);
            }

            var result = _mapper.Map<ForumPostResponseDTO>(updated);
            await HydrateMediaAsync(new List<ForumPostResponseDTO> { result }, ct);
            return result;
        }

        public async Task<bool> DeleteAsync(ulong id, CancellationToken ct = default)
        {
            // Xoá MediaLink trong DB (file Cloudinary đã do controller quyết định)
            var medias = await _db.MediaLinks
                .Where(m => m.OwnerType == MediaOwnerType.ForumPost && m.OwnerId == id)
                .ToListAsync(ct);

            if (medias.Count > 0)
            {
                _db.MediaLinks.RemoveRange(medias);
                await _db.SaveChangesAsync(ct);
            }

            return await _repo.DeleteAsync(id, ct);
        }

        // ================= Helpers =================

        // Chuẩn hoá slug (bỏ dấu, ký tự lạ, lowercase)
        private static string Slugify(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "post";

            var normalized = input.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc == UnicodeCategory.NonSpacingMark) continue;

                if (char.IsLetterOrDigit(ch)) sb.Append(ch);
                else if (ch == ' ' || ch == '-' || ch == '_') sb.Append('-');
                // bỏ qua ký tự khác
            }

            var slug = Regex.Replace(sb.ToString(), "-+", "-").Trim('-');
            if (string.IsNullOrEmpty(slug)) slug = "post";
            if (slug.Length > 200) slug = slug[..200];
            return slug;
        }

        // Đảm bảo slug duy nhất (tự cộng -2, -3, ...)
        private async Task<string> EnsureUniqueSlugAsync(string desired, ulong? ignoreId, CancellationToken ct)
        {
            var baseSlug = desired;
            var slug = baseSlug;
            var i = 1;

            while (await _db.ForumPosts.AnyAsync(p => p.Slug == slug && (!ignoreId.HasValue || p.Id != ignoreId.Value), ct))
            {
                i++;
                slug = $"{baseSlug}-{i}";
                if (slug.Length > 255) slug = slug[..255];
            }
            return slug;
        }

        private async Task SaveMediaLinksAsync(ulong postId, MediaUploadDTO? cover, IReadOnlyList<MediaUploadDTO> gallery, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var list = new List<MediaLink>();

            if (cover != null)
            {
                list.Add(new MediaLink
                {
                    OwnerType = MediaOwnerType.ForumPost,
                    OwnerId = postId,
                    ImageUrl = cover.Url,
                    ImagePublicId = cover.PublicId,
                    Purpose = MediaPurpose.Front,
                    SortOrder = 0,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            if (gallery != null && gallery.Count > 0)
            {
                int i = 1;
                foreach (var g in gallery)
                {
                    list.Add(new MediaLink
                    {
                        OwnerType = MediaOwnerType.ForumPost,
                        OwnerId = postId,
                        ImageUrl = g.Url,
                        ImagePublicId = g.PublicId,
                        Purpose = MediaPurpose.None,
                        SortOrder = i++,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
            }

            if (list.Count > 0)
            {
                _db.MediaLinks.AddRange(list);
                await _db.SaveChangesAsync(ct);
            }
        }

        private async Task HydrateMediaAsync(IReadOnlyList<ForumPostResponseDTO> posts, CancellationToken ct)
        {
            if (posts.Count == 0) return;
            var ids = posts.Select(p => p.Id).ToList();

            var medias = await _db.MediaLinks
                .Where(m => m.OwnerType == MediaOwnerType.ForumPost && ids.Contains(m.OwnerId))
                .OrderBy(m => m.SortOrder)
                .ToListAsync(ct);

            var lookup = medias.GroupBy(m => m.OwnerId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var p in posts)
            {
                if (!lookup.TryGetValue(p.Id, out var list)) continue;

                var cover = list.FirstOrDefault(x => x.Purpose == MediaPurpose.Front);
                p.CoverImageUrl = cover?.ImageUrl;

                p.GalleryImageUrls = list
                    .Where(x => x.Purpose != MediaPurpose.Front)
                    .OrderBy(x => x.SortOrder)
                    .Select(x => x.ImageUrl!)
                    .ToList();
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
