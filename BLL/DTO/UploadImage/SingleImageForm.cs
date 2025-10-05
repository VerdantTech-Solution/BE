using Microsoft.AspNetCore.Http;

namespace BLL.DTO.Upload;

public class SingleImageForm
{
    public IFormFile Image { get; set; } = default!;
}
