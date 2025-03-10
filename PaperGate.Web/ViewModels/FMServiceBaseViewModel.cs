namespace PaperGate.Web.ViewModels;

public abstract class FMServiceBaseViewModel
{
    public required List<IFormFile> Files { get; set; }
    public required string FolderPath { get; set; }
    public required FileType FileType { get; set; }
    public required FileCount FileCount { get; set; }
}
public class FMServiceUploadViewModel : FMServiceBaseViewModel
{
}
public class FMServiceAlterViewModel : FMServiceBaseViewModel
{
    public required string LastFilesNames { get; set; }
}
public enum FileType
{
    Video,
    Image
}
public enum FileCount
{
    Single,
    Multiple
}