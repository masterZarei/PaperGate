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
    protected TaskResult _taskResult;
    public FileManagementService(ILogger myLoggerRepository)
    {
        _taskResult = new();
        _myLoggerRepository = myLoggerRepository;
    }

    public async Task<TaskResult> Upload(FMServiceUploadViewModel dto)
    {
        try
        {
            Guard.Against.Null(dto);

            if (!Directory.Exists(dto.FolderPath))
                Directory.CreateDirectory(dto.FolderPath);

            return dto.FileType switch
            {
                FileType.Image => UploadImage(dto),
                _ => _taskResult,
            };
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, $"FileManagementService/UploadImages");
            _taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return _taskResult;
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
                _ => _taskResult,
            };
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, "FileManagementService/UploadImages");
            _taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return _taskResult;
        }
    }

    #region Image
    private async Task<TaskResult> AlterImages(FMServiceAlterViewModel dto)
    {
        try
        {
            Guard.Against.Null(dto);
            if (dto?.Files?.Count <= 0)
            {
                _taskResult.AddError("لطفا حداقل یک عکس برای محصول انتخاب کنید");
                return _taskResult;
            }
            if (dto?.Files?.Count > StaticValues.MaxImageUploadCount)
            {
                _taskResult.AddError("لطفا حداکثر چهار عکس انتخاب کنید");
                return _taskResult;
            }
            string ReturningFileNames = string.Empty;
            //Deleting Files
            var result = await Delete(dto.LastFilesNames, dto.FolderPath);
            if (result.Succeeded is false)
                _myLoggerRepository.Fatal(result.Errors.ToString(), "FileManagementService/AlterImages");

            //Here after deleting the last video _taskresults succeed prop turns TRUE
            //So we change it back to false
            _taskResult.Succeeded = false;

            foreach (var item in dto?.Files)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), dto.FolderPath);

                #region Validation
                //Check files Sizes
                if (item.Length > StaticValues.MaxImageUploadSize)
                {
                    _taskResult.AddError("فایل انتخابی بیشتر از 20 مگابایت است");
                    return _taskResult;
                }
                //Check Files Types
                string fileExtension = Path.GetExtension(item.FileName);
                if (!string.IsNullOrEmpty(fileExtension) && FileFormats.CheckImageFormats(fileExtension) == false)
                {
                    _taskResult.AddError("فایل ورودی نامعتبر است");
                    return _taskResult;
                }
                #endregion
                string newName = NameGenerator.FilenameGenerate(Path.GetFileNameWithoutExtension(item.FileName), fileExtension);
                string fileNameWithPath = Path.Combine(path, newName);

                using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                item.CopyTo(stream);

                //Add File Path to ImageAdderss Column
                if (dto.FileCount is FileCount.Multiple)
                    ReturningFileNames += $"{newName},";
                else
                    ReturningFileNames += $"{newName}";
            }
            _taskResult.Succeeded = true;
            _taskResult.Result = ReturningFileNames;
            return _taskResult;



        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, "FileManagementService/AlterImages");
            _taskResult.Succeeded = false;
            _taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return _taskResult;
        }
    }
    private TaskResult UploadImage(FMServiceUploadViewModel dto)
    {
        try
        {
            if (dto?.Files?.Count <= 0)
            {
                _taskResult.AddError("لطفا حداقل یک عکس برای محصول انتخاب کنید");
                return _taskResult;
            }
            if (dto?.Files?.Count > StaticValues.MaxImageUploadCount)
            {
                _taskResult.AddError("لطفا حداکثر چهار عکس انتخاب کنید");
                return _taskResult;
            }
            string ReturningFileNames = string.Empty;
            foreach (var item in dto?.Files)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), dto.FolderPath);

                #region Validation
                //Check Files Types
                string fileExtension = Path.GetExtension(item.FileName);
                if (!string.IsNullOrEmpty(fileExtension) && FileFormats.CheckImageFormats(fileExtension) == false)
                {
                    _taskResult.AddError("تصویر ورودی نامعتبر است");
                    return _taskResult;
                }
                //Check files Sizes
                if (item.Length > StaticValues.MaxImageUploadSize)
                {
                    _taskResult.AddError("تصویر انتخابی بیشتر از 20 مگابایت است");
                    return _taskResult;
                }
                #endregion
                string newName = NameGenerator.FilenameGenerate(Path.GetFileNameWithoutExtension(item.FileName), fileExtension);
                string fileNameWithPath = Path.Combine(path, newName);

                using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                item.CopyTo(stream);

                //Add File Path to ImageAdderss Column
                if (dto.FileCount is FileCount.Multiple)
                    ReturningFileNames += $"{newName},";
                else
                    ReturningFileNames += $"{newName}";
            }
            _taskResult.Succeeded = true;
            _taskResult.Result = ReturningFileNames;
            return _taskResult;


        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, "FileManagementService/UploadImages", ex.ToString());
            _taskResult.AddError("در فرآیند آپلود عکس خطایی رخ داد");
            return _taskResult;
        }
    }
    #endregion

    #region General
    public async Task<TaskResult> Delete(string files, string savePath)
    {
        try
        {
            Guard.Against.NullOrEmpty(savePath);

            if (!string.IsNullOrEmpty(files) && files.Length >= 0)
            {
                foreach (var pics in files.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    string path = Path.Combine(savePath, pics);
                    if (File.Exists(path))
                        File.Delete(path);
                }
            }
            _taskResult.Succeeded = true;
            return _taskResult;
        }
        catch (Exception ex)
        {
            _myLoggerRepository.Fatal(ex, $"FileManagementService/Delete", ex.ToString());
            _taskResult.AddError("در فرآیند حذف عکس خطایی رخ داد");
            return _taskResult;
        }
    }
    #endregion



}
