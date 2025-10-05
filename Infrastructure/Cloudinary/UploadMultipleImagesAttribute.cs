using Microsoft.AspNetCore.Http;
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
        var results = new List<UploadResultDto>();

        if (http.Request.HasFormContentType)
        {
            var files = http.Request.Form.Files.GetFiles(_formField);
            if (files.Any())
            {
                var cloudinary = http.RequestServices.GetRequiredService<ICloudinaryService>();

                foreach (var file in files)
                {
                    var uploaded = await cloudinary.UploadAsync(file, _folder, http.RequestAborted);
                    if (uploaded is not null)
                        results.Add(uploaded);
                }

                if (results.Any())
                    http.Items["uploadedImages"] = results;
            }
        }

        await next();
    }
}
