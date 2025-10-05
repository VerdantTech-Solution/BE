using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.ForumComment
{
   
    public class ForumCommentCreateDTO
    {
        [Required]
        public ulong ForumPostId { get; set; }

        public ulong? ParentId { get; set; }

        [Required, MinLength(1)]
        public string Content { get; set; } = string.Empty;

        
    }
}
