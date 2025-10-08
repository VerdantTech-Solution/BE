using BLL.DTO.ForumCategory;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")] 
    public class ForumCategoryController : ControllerBase
    {
        private readonly IForumCategoryService _service;

        public ForumCategoryController(IForumCategoryService service)
        {
            _service = service;
        }


        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Get All Forum Category")]
        [EndpointDescription("List tất cả danh mục thảo luận trong diễn đàn.")]
        public async Task<ActionResult<IReadOnlyList<ForumCategoryResponseDTO>>> GetAll(CancellationToken ct)
        {
            var data = await _service.GetAllAsync(ct);
            return Ok(data);
        }
        /// <summary>Tạo mới forum category</summary>
        [HttpPost]
        [EndpointSummary("Create Forum Category")]
        [EndpointDescription("Tạo mới một danh mục thảo luận trong diễn đàn.")]
        public async Task<ActionResult<ForumCategoryResponseDTO>> Create(
            [FromBody] ForumCategoryCreateDTO dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto, ct);
            return Ok(created);
        }

        /// <summary>Cập nhật forum category</summary>
        [HttpPut("{id}")]
        [EndpointSummary("Update Forum Category")]
        [EndpointDescription("Cập nhật thông tin một danh mục thảo luận theo ID.")]
        public async Task<ActionResult<ForumCategoryResponseDTO>> Update(
            ulong id,
            [FromBody] ForumCategoryUpdateDTO dto,
            CancellationToken ct)
        {
            if (id != dto.Id) return BadRequest("Id không khớp");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(dto, ct);
            return Ok(updated);
        }

        /// <summary>Xóa forum category</summary>
        [HttpDelete("{id}")]
        [EndpointSummary("Delete Forum Category")]
        [EndpointDescription("Xóa danh mục thảo luận theo ID.")]
        public async Task<ActionResult> Delete(
            ulong id,
            CancellationToken ct)
        {
            var result = await _service.DeleteAsync(id, ct);
            if (!result) return NotFound("Không tồn tại");
            return NoContent();
        }
    }
}
