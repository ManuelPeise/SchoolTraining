namespace Shared.Models
{
    public class FileResponse
    {
        public string ContentType { get; set; } = "application/octet-stream";
        public string FileName { get; set; } = string.Empty;
        public List<byte> Bytes { get; set; } = new();
    }
}
