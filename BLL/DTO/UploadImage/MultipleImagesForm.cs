using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BLL.DTO.Upload;

public class MultipleImagesForm
{
    public List<IFormFile> Images { get; set; } = new();
}
