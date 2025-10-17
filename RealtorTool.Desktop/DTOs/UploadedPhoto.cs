using Avalonia.Media.Imaging;

namespace RealtorTool.Desktop.DTOs;

public class UploadedPhoto
{
    public string Id { get; set; }
    public string FileName { get; set; } = null!;
    public long FileSize { get; set; }
    public string FileSizeFormatted { get; set; } = null!;
    public byte[] FileData { get; set; } = null!;
    public Bitmap? Preview { get; set; }
    public string ContentType { get; set; } = null!;
    public bool IsMain { get; set; }
    public string FilePath { get; set; }
}