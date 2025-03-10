using PaperGate.Shared.ReturnTypes;
using PaperGate.Web.ViewModels;

namespace PaperGate.Web.Interfaces.Services;

public interface IFileManagementService
{
    public Task<TaskResult> Upload(FMServiceUploadViewModel dto);
    public Task<TaskResult> Alter(FMServiceAlterViewModel dto);
    public Task<TaskResult> Delete(string files, string savePath);
}
