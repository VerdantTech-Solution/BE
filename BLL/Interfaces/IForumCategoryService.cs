using BLL.DTO.ForumCategory;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IForumCategoryService
    {
        Task<IReadOnlyList<ForumCategoryResponseDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ForumCategoryResponseDTO> CreateAsync(ForumCategoryCreateDTO dto,CancellationToken cancellationToken = default);
        Task<ForumCategoryResponseDTO> UpdateAsync(ForumCategoryUpdateDTO dto,CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(ulong id,CancellationToken cancellationToken = default);
    }
}
