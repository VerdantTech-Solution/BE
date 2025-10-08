using DAL.Data.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.ForumPost
{
    public class ForumPostResponseDTO
    {
        public ulong Id { get; set; }
        public ulong ForumCategoryId { get; set; }
        public ulong UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public List<ContentBlock> Content { get; set; } = new();
        public string? Tags { get; set; }
        public long ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsPinned { get; set; }
        public ForumPostStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CoverImageUrl { get; set; }
        public IReadOnlyList<string> GalleryImageUrls { get; set; } = new List<string>();

    }
}
