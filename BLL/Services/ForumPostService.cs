using AutoMapper;
using BLL.DTO;
using BLL.DTO.ForumPost;
using BLL.Interfaces;
using DAL.Data;
using DAL.Data.Models;
using DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ForumPostService : IForumPostService
    {
        private readonly IForumPostRepository _repo;
        private readonly IMapper _mapper;

        public ForumPostService(IForumPostRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ForumPostResponseDTO>> GetAllAsync(int page, int pageSize, ForumPostStatus? status = null, CancellationToken ct = default)
        {
            var (items, total) = await _repo.GetAllAsync(page, pageSize, status, useNoTracking: true, ct);
            var dtoItems = _mapper.Map<List<ForumPostResponseDTO>>(items);
            return ToPagedResponse(dtoItems, total, page, pageSize);
        }

        public async Task<PagedResponse<ForumPostResponseDTO>> GetByCategoryAsync( ulong categoryId, int page, int pageSize, ForumPostStatus? status = null,  CancellationToken ct = default)
        {
            var (items, total) = await _repo.GetByCategoryAsync(categoryId, page, pageSize, status, useNoTracking: true, ct);
            var dtoItems = _mapper.Map<List<ForumPostResponseDTO>>(items);
            return ToPagedResponse(dtoItems, total, page, pageSize);
        }

        public async Task<ForumPostResponseDTO?> GetByIdAsync(ulong id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, useNoTracking: true, ct);
            return entity == null ? null : _mapper.Map<ForumPostResponseDTO>(entity);
        }

        public async Task<ForumPostResponseDTO?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var entity = await _repo.GetBySlugAsync(slug, useNoTracking: true, ct);
            return entity == null ? null : _mapper.Map<ForumPostResponseDTO>(entity);
        }

        public async Task<ForumPostResponseDTO> CreateAsync(ForumPostCreateDTO dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<ForumPost>(dto);
            entity.ViewCount = 0;
            entity.LikeCount = 0;
            entity.DislikeCount = 0;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var created = await _repo.CreateAsync(entity, ct);
            return _mapper.Map<ForumPostResponseDTO>(created);
        }

        public async Task<ForumPostResponseDTO> UpdateAsync(ForumPostUpdateDTO dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(dto.Id, useNoTracking: false, ct);
            if (entity == null) throw new Exception("Forum post not found");

            // cập nhật field cho phép sửa
            entity.ForumCategoryId = dto.ForumCategoryId;
            entity.Title = dto.Title;
            entity.Slug = dto.Slug;
            entity.Content = dto.Content;
            entity.Tags = dto.Tags;
            entity.IsPinned = dto.IsPinned;
            entity.Status = dto.Status;
            entity.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(entity, ct);
            return _mapper.Map<ForumPostResponseDTO>(updated);
        }

        public async Task<bool> DeleteAsync(ulong id, CancellationToken ct = default)
        {
            return await _repo.DeleteAsync(id, ct);
        }

        // helper build PagedResponse
        private static PagedResponse<T> ToPagedResponse<T>(List<T> items, int totalRecords, int page, int pageSize)
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
