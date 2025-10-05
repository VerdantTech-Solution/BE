using System.ComponentModel.DataAnnotations;
using DAL.Data;
using DAL.Data.Models;

namespace BLL.DTO.ForumComment
{
    public class ForumCommentUpdateDTO
    {
        [Required, MinLength(1)]
        public string Content { get; set; } = string.Empty;
    }
}
