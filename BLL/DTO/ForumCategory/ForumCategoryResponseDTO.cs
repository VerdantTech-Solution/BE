using System;

namespace BLL.DTO.ForumCategory
{
    public class ForumCategoryResponseDTO
    {
        public ulong Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
