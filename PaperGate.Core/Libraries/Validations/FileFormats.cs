namespace PaperGate.Core.Libraries.Validations;
public static class FileFormats
{
    private static readonly List<string> _imageFormatsList =
        [
             new(".jpg"),
             new(".jpeg"),
             new(".png"),
             new(".webp"),
             new(".gif")
        ];
    private static readonly List<string> _videoFormatsList =
        [
            new(".mp4"),
            new(".avi"),
            new(".wmv"),
            new(".mkv"),
            new(".3gp"),
        ];
    public static bool CheckImageFormats(string input) => _imageFormatsList.Any(a => a == input || a.Equals(input, StringComparison.CurrentCultureIgnoreCase));
    public static bool CheckVideoFormats(string input) => _videoFormatsList.Any(a => a == input || a.Equals(input, StringComparison.CurrentCultureIgnoreCase));

}
