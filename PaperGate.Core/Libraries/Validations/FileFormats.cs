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

    public static bool CheckImageFormats(string input) => _imageFormatsList.Any(a => a.Equals(input, StringComparison.OrdinalIgnoreCase));

    public static bool CheckImageSignature(Stream stream)
    {
        if (stream is null)
            return false;

        byte[] header = new byte[12];
        int offset = 0;
        while (offset < header.Length)
        {
            int read = stream.Read(header, offset, header.Length - offset);
            if (read == 0)
                break;
            offset += read;
        }

        if (stream.CanSeek)
            stream.Position = 0;

        return CheckImageSignature(header, offset);
    }

    public static bool CheckImageSignature(byte[] header, int length)
    {
        if (header is null || length < 3)
            return false;

        // JPEG: FF D8 FF
        if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
            return true;

        // PNG: 89 50 4E 47 0D 0A 1A 0A
        if (length >= 8 &&
            header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47 &&
            header[4] == 0x0D && header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A)
            return true;

        // GIF: GIF87a / GIF89a
        if (length >= 6 &&
            header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46 &&
            header[3] == 0x38 && (header[4] == 0x37 || header[4] == 0x39) && header[5] == 0x61)
            return true;

        // WebP: RIFF .... WEBP
        if (length >= 12 &&
            header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46 &&
            header[8] == 0x57 && header[9] == 0x45 && header[10] == 0x42 && header[11] == 0x50)
            return true;

        return false;
    }
}
