using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.ForumCategory
{
    public class ForumCategoryCreateDTO
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        // Cho phép bật/tắt ngay khi tạo (mặc định true)
        public bool IsActive { get; set; } = true;
    }
}
