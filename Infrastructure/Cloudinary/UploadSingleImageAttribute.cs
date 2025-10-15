using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cloudinary;

[AttributeUsage(AttributeTargets.Method)]
public class UploadSingleImageAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _formField;
    private readonly string? _folder;

    public UploadSingleImageAttribute(string formField, string? folder = null)
    {
        _formField = formField;
        _folder = folder;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var http = context.HttpContext;

        if (http.Request.HasFormContentType)
        {
            var file = http.Request.Form.Files.GetFile(_formField);
            if (file != null)
            {
                var svc = http.RequestServices.GetRequiredService<ICloudinaryService>();
                var uploaded = await svc.UploadAsync(file, _folder, http.RequestAborted);
                if (uploaded is not null)
                {
                    http.Items["uploadedImageUrl"] = uploaded.Url;
                    http.Items["uploadedImagePublicId"] = uploaded.PublicId;
                }
            }
        }

        await next();
    }
}
