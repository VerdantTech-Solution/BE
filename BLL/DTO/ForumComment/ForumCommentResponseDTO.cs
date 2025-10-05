using System;
using System.Collections.Generic;
using DAL.Data;
using DAL.Data.Models;

namespace BLL.DTO.ForumComment
{
    //DTO trả về comment (hỗ trợ children nếu cần)
    public class ForumCommentResponseDTO
    {
        public ulong Id { get; set; }
        public ulong ForumPostId { get; set; }
        public ulong UserId { get; set; }
        public ulong? ParentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public ForumCommentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ForumCommentResponseDTO> Children { get; set; } = new();
    }
}
