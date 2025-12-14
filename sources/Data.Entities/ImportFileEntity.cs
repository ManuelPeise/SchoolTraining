using Shared.Enums;

namespace Data.Entities
{
    public class ImportFileEntity: AEntityBase
    {
        public string FileName { get; set; } = string.Empty;
        public DateTime FileDate { get; set; }
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
        public FileImportTypeEnum FileType { get; set; }
        public ImportStatusEnum Status { get; set; }
    }
}
