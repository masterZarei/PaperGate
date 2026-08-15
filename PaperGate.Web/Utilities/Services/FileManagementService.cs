using Ardalis.GuardClauses;
using PaperGate.Core.Libraries.Generators;
using PaperGate.Core.Libraries.Validations;
using PaperGate.Shared.ReturnTypes;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Libraries;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Utilities.Services;

public class FileManagementService : IFileManagementService
{
    private readonly ILogger _myLoggerRepository;

    public FileManagementService(ILogger myLoggerRepository)
    {
        _myLoggerRepository = myLoggerRepository;
    }

    public Task<TaskResult> Upload(FMServiceUploadViewModel dto)
    {
        try
        {
            Guard.Against.Null(dto);

            if (!Directory.Exists(dto.FolderPath))
                Directory.CreateDirectory(dto.FolderPath);

            return Task.FromResult(UploadImage(dto));
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, $"FileManagementService/UploadImages");
            var taskResult = new TaskResult();
            taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return Task.FromResult(taskResult);
        }
    }

    public async Task<TaskResult> Alter(FMServiceAlterViewModel dto)
    {
        try
        {
            Guard.Against.Null(dto);

            if (!Directory.Exists(dto.FolderPath))
                Directory.CreateDirectory(dto.FolderPath);

            return dto.FileType switch
            {
                FileType.Image => await AlterImages(dto),
                _ => new TaskResult(),
            };
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, "FileManagementService/UploadImages");
            var taskResult = new TaskResult();
            taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return taskResult;
        }
    }

    #region Image
    private async Task<TaskResult> AlterImages(FMServiceAlterViewModel dto)
    {
        var taskResult = new TaskResult();
        try
        {
            Guard.Against.Null(dto);
            if (dto?.Files?.Count <= 0)
            {
                taskResult.AddError("لطفا حداقل یک عکس برای محصول انتخاب کنید");
                return taskResult;
            }
            if (dto?.Files?.Count > StaticValues.MaxImageUploadCount)
            {
                taskResult.AddError("لطفا حداکثر چهار عکس انتخاب کنید");
                return taskResult;
            }
            string returningFileNames = string.Empty;
            var deleteResult = await Delete(dto.LastFilesNames, dto.FolderPath);
            if (deleteResult.Succeeded is false)
                _myLoggerRepository.Fatal(deleteResult.Errors.ToString(), "FileManagementService/AlterImages");

            foreach (var item in dto.Files)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), dto.FolderPath);

                #region Validation
                if (item.Length > StaticValues.MaxImageUploadSize)
                {
                    taskResult.AddError("فایل انتخابی بیشتر از 20 مگابایت است");
                    return taskResult;
                }
                string fileExtension = Path.GetExtension(item.FileName);
                if (!string.IsNullOrEmpty(fileExtension) && FileFormats.CheckImageFormats(fileExtension) == false)
                {
                    taskResult.AddError("فایل ورودی نامعتبر است");
                    return taskResult;
                }
                using (var sourceStream = item.OpenReadStream())
                {
                    if (FileFormats.CheckImageSignature(sourceStream) == false)
                    {
                        taskResult.AddError("فایل ورودی نامعتبر است");
                        return taskResult;
                    }

                    string newName = NameGenerator.FilenameGenerate(Path.GetFileNameWithoutExtension(item.FileName), fileExtension);
                    string fileNameWithPath = Path.Combine(path, newName);

                    using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                    await sourceStream.CopyToAsync(stream);

                    if (dto.FileCount is FileCount.Multiple)
                        returningFileNames += $"{newName},";
                    else
                        returningFileNames += $"{newName}";
                }
                #endregion
            }
            taskResult.Succeeded = true;
            taskResult.Result = returningFileNames;
            return taskResult;
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, "FileManagementService/AlterImages");
            taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return taskResult;
        }
    }

    private TaskResult UploadImage(FMServiceUploadViewModel dto)
    {
        var taskResult = new TaskResult();
        try
        {
            if (dto?.Files?.Count <= 0)
            {
                taskResult.AddError("لطفا حداقل یک عکس برای محصول انتخاب کنید");
                return taskResult;
            }
            if (dto?.Files?.Count > StaticValues.MaxImageUploadCount)
            {
                taskResult.AddError("لطفا حداکثر چهار عکس انتخاب کنید");
                return taskResult;
            }
            string returningFileNames = string.Empty;
            foreach (var item in dto.Files)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), dto.FolderPath);

                #region Validation
                string fileExtension = Path.GetExtension(item.FileName);
                if (!string.IsNullOrEmpty(fileExtension) && FileFormats.CheckImageFormats(fileExtension) == false)
                {
                    taskResult.AddError("تصویر ورودی نامعتبر است");
                    return taskResult;
                }
                if (item.Length > StaticValues.MaxImageUploadSize)
                {
                    taskResult.AddError("تصویر انتخابی بیشتر از 20 مگابایت است");
                    return taskResult;
                }
                #endregion

                using (var sourceStream = item.OpenReadStream())
                {
                    if (FileFormats.CheckImageSignature(sourceStream) == false)
                    {
                        taskResult.AddError("تصویر ورودی نامعتبر است");
                        return taskResult;
                    }

                    string newName = NameGenerator.FilenameGenerate(Path.GetFileNameWithoutExtension(item.FileName), fileExtension);
                    string fileNameWithPath = Path.Combine(path, newName);

                    using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                    sourceStream.CopyTo(stream);

                    if (dto.FileCount is FileCount.Multiple)
                        returningFileNames += $"{newName},";
                    else
                        returningFileNames += $"{newName}";
                }
            }
            taskResult.Succeeded = true;
            taskResult.Result = returningFileNames;
            return taskResult;
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, "FileManagementService/UploadImages", ex.ToString());
            taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return taskResult;
        }
    }
    #endregion

    #region General
    public async Task<TaskResult> Delete(string files, string savePath)
    {
        var taskResult = new TaskResult();
        try
        {
            Guard.Against.NullOrEmpty(savePath);

            if (!string.IsNullOrEmpty(files))
            {
                foreach (var pics in files.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    string path = Path.Combine(savePath, pics);
                    if (File.Exists(path))
                        File.Delete(path);
                }
            }
            taskResult.Succeeded = true;
            return taskResult;
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, $"FileManagementService/Delete", ex.ToString());
            taskResult.AddError("در فرآیند حذف عکس خطایی رخ داد");
            return taskResult;
        }
    }
    #endregion
}
