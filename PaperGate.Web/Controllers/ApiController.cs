using Microsoft.AspNetCore.Mvc;
using PaperGate.Infra.Data;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Libraries;
using PaperGate.Web.ViewModels;

namespace PaperGate.Web.Controllers;
[Route("[controller]/[action]")]
[ApiController]
public class ApiController : Controller
{
    private readonly IFileManagementService _fileManagementService;

    public ApiController( IFileManagementService fileManagementService)
    {
        _fileManagementService = fileManagementService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadPaperImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "فایلی ارسال نشده است.", location = "" });
        }

        if (file.Length > 5 * 1024 * 1024) // حداکثر 5 مگابایت
        {
            return BadRequest(new { error = "حجم فایل بیش از حد مجاز است.", location = "" });
        }

        // بررسی فرمت فایل
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { error = "فرمت فایل معتبر نیست.", location = "" });
        }

        // ارسال به سرویس مدیریت فایل
        var result = await _fileManagementService.Upload(new FMServiceUploadViewModel
        {
            FileCount = FileCount.Single,
            Files = [file],
            FileType = FileType.Image,
            FolderPath = StaticValues.PaperImagesPath
        });

        if (result.Succeeded is false)
        {
            return BadRequest(new { error = result.Errors.FirstOrDefault() ?? "آپلود تصویر ناموفق بود.", location = "" });
        }

        // تولید لینک تصویر
        var fileUrl = $"{Request.Scheme}://{Request.Host}/Assets/Pictures/Papers/{result.Result}";
        return new JsonResult(new { location = fileUrl });
    }

}