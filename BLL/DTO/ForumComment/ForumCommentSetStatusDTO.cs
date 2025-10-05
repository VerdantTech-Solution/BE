using System.ComponentModel.DataAnnotations;
using DAL.Data;
using DAL.Data.Models;

namespace BLL.DTO.ForumComment
{
   
    public class ForumCommentSetStatusDTO
    {
        [Required] public ulong Id { get; set; }
        [Required] public ForumCommentStatus Status { get; set; }
    }
}
