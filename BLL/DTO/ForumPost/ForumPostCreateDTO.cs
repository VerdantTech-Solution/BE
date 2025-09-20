using DAL.Data.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.ForumPost
{
    public class ForumPostCreateDTO
    {
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

        public bool IsPinned { get; set; } = false;

        public ForumPostStatus Status { get; set; } = ForumPostStatus.Visible;
    }
}
