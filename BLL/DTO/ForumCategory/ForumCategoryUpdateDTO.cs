using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.ForumCategory
{
    public class ForumCategoryUpdateDTO
    {
        [Required]
        public ulong Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
