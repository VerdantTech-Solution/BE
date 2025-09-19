using AutoMapper;
using BLL.DTO.ForumCategory;
using BLL.Interfaces;
using DAL.Data.Models;
using DAL.IRepository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ForumCategoryService : IForumCategoryService
    {
        private readonly IForumCategoryRepository _repo;
        private readonly IMapper _mapper;

        public ForumCategoryService(IForumCategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ForumCategoryResponseDTO> CreateAsync(ForumCategoryCreateDTO dto,CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<ForumCategory>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var created = await _repo.CreateAsync(entity, cancellationToken);
            return _mapper.Map<ForumCategoryResponseDTO>(created);
        }

        public async Task<ForumCategoryResponseDTO> UpdateAsync(ForumCategoryUpdateDTO dto,CancellationToken cancellationToken = default)
        {
            var entity = await _repo.GetByIdAsync(dto.Id, useNoTracking: false, cancellationToken);
            if (entity == null) throw new Exception("Forum category not found");

            // cập nhật field cho phép sửa
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(entity, cancellationToken);
            return _mapper.Map<ForumCategoryResponseDTO>(updated);
        }

        public async Task<bool> DeleteAsync(ulong id,CancellationToken cancellationToken = default)
        {
            return await _repo.DeleteAsync(id, cancellationToken);
        }
    }
}
