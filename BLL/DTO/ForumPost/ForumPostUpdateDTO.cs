using DAL.Data.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BLL.DTO.ForumPost
{
    public class ForumPostUpdateDTO
    {
        [Required]
        public ulong Id { get; set; }

        [Required]
        public ulong ForumCategoryId { get; set; }

        [Required, StringLength(255)]
        public string Title { get; set; } = null!;

        [Required, StringLength(255)]
        public string Slug { get; set; } = null!;

        [Required]
        public List<ContentBlock> Content { get; set; } = new();

        [StringLength(500)]
        public string? Tags { get; set; }

        public bool IsPinned { get; set; }

        public ForumPostStatus Status { get; set; }

        // NEW: file upload (tùy chọn, khi dùng multipart/form-data)
        public IFormFile? CoverImage { get; set; }
        public List<IFormFile>? Images { get; set; }

        // NEW: cho phép client yêu cầu xóa ảnh cũ theo publicId
        public List<string>? RemovePublicIds { get; set; }
    }
}
