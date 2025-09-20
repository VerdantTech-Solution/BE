using BLL.DTO;
using BLL.DTO.ForumPost;
using DAL.Data;
using DAL.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IForumPostService
    {
        Task<PagedResponse<ForumPostResponseDTO>> GetAllAsync(int page, int pageSize, ForumPostStatus? status = null, CancellationToken ct = default);
        Task<ForumPostResponseDTO?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<PagedResponse<ForumPostResponseDTO>> GetByCategoryAsync(ulong categoryId, int page, int pageSize, ForumPostStatus? status = null, CancellationToken ct = default);
        Task<ForumPostResponseDTO?> GetByIdAsync(ulong id, CancellationToken ct = default);
        Task<ForumPostResponseDTO> CreateAsync(ForumPostCreateDTO dto, CancellationToken ct = default);
        Task<ForumPostResponseDTO> UpdateAsync(ForumPostUpdateDTO dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(ulong id, CancellationToken ct = default);
    }
}
