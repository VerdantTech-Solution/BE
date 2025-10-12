using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cloudinary;

[AttributeUsage(AttributeTargets.Method)]
public class UploadMultipleImagesAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _formField;
    private readonly string? _folder;

    public UploadMultipleImagesAttribute(string formField, string? folder = null)
    {
        _formField = formField;
        _folder = folder;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var http = context.HttpContext;

        if (http.Request.HasFormContentType)
        {
            var files = http.Request.Form.Files.GetFiles(_formField);
            if (files.Count > 0)
            {
                var svc = http.RequestServices.GetRequiredService<ICloudinaryService>();
                var uploaded = await svc.UploadManyAsync(files, _folder, http.RequestAborted);
                if (uploaded.Count > 0)
                {
                    http.Items["uploadedImages"] = uploaded;                         // List<UploadResultDto>
                    http.Items["uploadedImageUrls"] = uploaded.Select(x => x.Url).ToList();
                    http.Items["uploadedImagePublicIds"] = uploaded.Select(x => x.PublicUrl).ToList();
                }
            }
        }

        await next();
    }
}
