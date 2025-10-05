using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.DTO.ForumComment;
// ĐỔI nếu namespace PagedResponse khác:
using BLL.DTO.Shared;                   // chứa PagedResponse<T>
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class ForumCommentService : IForumCommentService
    {
        private readonly IForumCommentRepository _repo;
        private readonly IMapper _mapper;

        public ForumCommentService(IForumCommentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ====================== Create ======================

        public async Task<ForumCommentResponseDTO> CreateAsync(
            ForumCommentCreateDTO dto,
            ulong currentUserId,
            CancellationToken ct = default)
        {
            var entity = _mapper.Map<ForumComment>(dto);
            entity.UserId = currentUserId;

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);

            var created = await _repo.GetByIdAsync(entity.Id, ct);
            return _mapper.Map<ForumCommentResponseDTO>(created!);
        }

        // ====================== Read ======================

        public async Task<ForumCommentResponseDTO?> GetByIdAsync(
            ulong id,
            bool deep = false,
            CancellationToken ct = default)
        {
            if (!deep)
            {
                var entity = await _repo.GetByIdAsync(id, ct);
                if (entity == null) return null;

                var dto = _mapper.Map<ForumCommentResponseDTO>(entity);

                // children 1 cấp (nếu repo đã Include InverseParent)
                if (entity.InverseParent?.Count > 0)
                {
                    dto.Children = _mapper.Map<List<ForumCommentResponseDTO>>(
                        entity.InverseParent
                              .Where(c => c.Status == ForumCommentStatus.Visible)
                              .OrderBy(c => c.CreatedAt)
                              .ToList());
                }

                return dto;
            }
            else
            {
                var flat = await _repo.GetThreadAsync(id, null, ForumCommentStatus.Visible, ct);
                if (flat == null || flat.Count == 0) return null;

                return BuildTree(flat, id);
            }
        }

        public async Task<PagedResponse<ForumCommentResponseDTO>> GetByPostAsync(
            ulong postId,
            int page,
            int pageSize,
            bool deep = false,
            CancellationToken ct = default)
        {
            if (!deep)
            {
                var (roots, total) = await _repo.GetByPostIdAsync(
                    postId, page, pageSize,
                    includeChildren: true,
                    status: ForumCommentStatus.Visible,
                    ct: ct);

                var rootDtos = roots.Select(r => _mapper.Map<ForumCommentResponseDTO>(r)).ToList();

                // map children 1 cấp
                for (int i = 0; i < roots.Count; i++)
                {
                    var children = roots[i].InverseParent
                        .Where(ch => ch.Status == ForumCommentStatus.Visible)
                        .OrderBy(ch => ch.CreatedAt)
                        .ToList();

                    rootDtos[i].Children = _mapper.Map<List<ForumCommentResponseDTO>>(children);
                }

                return ToPagedResponse(rootDtos, total, page, pageSize);
            }
            else
            {
                var (roots, total) = await _repo.GetByPostIdAsync(
                    postId, page, pageSize,
                    includeChildren: false,
                    status: ForumCommentStatus.Visible,
                    ct: ct);

                var list = new List<ForumCommentResponseDTO>();

                foreach (var root in roots)
                {
                    var flat = await _repo.GetThreadAsync(
                        root.Id, null, ForumCommentStatus.Visible, ct);

                    if (flat.Count > 0)
                        list.Add(BuildTree(flat, root.Id));
                }

                return ToPagedResponse(list, total, page, pageSize);
            }
        }

        public async Task<IReadOnlyList<ForumCommentResponseDTO>> GetChildrenAsync(
            ulong parentId,
            CancellationToken ct = default)
        {
            var items = await _repo.GetChildrenAsync(parentId, ForumCommentStatus.Visible, ct);
            return _mapper.Map<IReadOnlyList<ForumCommentResponseDTO>>(items);
        }

        public async Task<PagedResponse<ForumCommentResponseDTO>> SearchAsync(
            string? keyword,
            ulong? postId,
            ulong? userId,
            ForumCommentStatus? status,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var (items, total) = await _repo.SearchAsync(
                keyword ?? string.Empty, postId, userId, status, page, pageSize, ct);

            var dtos = _mapper.Map<IReadOnlyList<ForumCommentResponseDTO>>(items).ToList();
            return ToPagedResponse(dtos, total, page, pageSize);
        }

        public Task<int> CountByPostAsync(
            ulong postId,
            ForumCommentStatus? status = ForumCommentStatus.Visible,
            CancellationToken ct = default)
            => _repo.CountByPostAsync(postId, status, ct);

        // ====================== Update ======================

        public async Task<bool> UpdateContentAsync(
            ulong id,
            ForumCommentUpdateDTO dto,
            ulong currentUserId,
            bool isModerator = false,
            CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return false;

            // quyền: tác giả hoặc mod
            if (entity.UserId != currentUserId && !isModerator) return false;

            entity.Content = dto.Content;
            await _repo.UpdateAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return true;
        }

        // ====================== Moderation ======================

        public async Task<bool> SetStatusAsync(
            ForumCommentSetStatusDTO dto,
            ulong actingUserId,
            bool isModerator = false,
            CancellationToken ct = default)
        {
            if (!isModerator) return false;

            var ok = await _repo.SetStatusAsync(dto.Id, dto.Status, ct);
            if (ok) await _repo.SaveChangesAsync(ct);
            return ok;
        }

        // ====================== Delete ======================

        public async Task<bool> DeleteAsync(
            ulong id,
            bool hardDelete,
            ulong actingUserId,
            bool isModerator = false,
            CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return false;

            if (entity.UserId != actingUserId && !isModerator) return false;

            if (hardDelete)
                await _repo.DeleteAsync(id, ct);
            else
                await _repo.SoftDeleteAsync(id, ct);

            await _repo.SaveChangesAsync(ct);
            return true;
        }

        // ====================== Reactions ======================

        public async Task<bool> LikeAsync(ulong id, int delta = 1, CancellationToken ct = default)
        {
            var ok = await _repo.IncrementLikeAsync(id, delta, ct);
            if (ok) await _repo.SaveChangesAsync(ct);
            return ok;
        }

        public async Task<bool> DislikeAsync(ulong id, int delta = 1, CancellationToken ct = default)
        {
            var ok = await _repo.IncrementDislikeAsync(id, delta, ct);
            if (ok) await _repo.SaveChangesAsync(ct);
            return ok;
        }

        // ====================== Helpers ======================

        private static PagedResponse<T> ToPagedResponse<T>(IReadOnlyList<T> data, int total, int page, int pageSize)
        {
            var totalPages = pageSize <= 0 ? 0 : (int)Math.Ceiling(total / (double)pageSize);
            return new PagedResponse<T>
            {
                Data = data.ToList(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = total,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }

        private ForumCommentResponseDTO BuildTree(IReadOnlyList<ForumComment> nodes, ulong rootId)
        {
            var dict = nodes.ToDictionary(n => n.Id, n => _mapper.Map<ForumCommentResponseDTO>(n));

            foreach (var n in nodes)
            {
                if (n.ParentId.HasValue && dict.TryGetValue(n.ParentId.Value, out var parentDto))
                {
                    parentDto.Children.Add(dict[n.Id]);
                }
            }

            void SortRec(ForumCommentResponseDTO node)
            {
                node.Children = node.Children
                    .OrderBy(c => c.CreatedAt)
                    .ToList();

                foreach (var ch in node.Children)
                    SortRec(ch);
            }

            var root = dict[rootId];
            SortRec(root);
            return root;
        }
    }
}
