using Shared.Enums;

namespace Shared.Models.Import
{
    public class ImportFileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime FileDate { get; set; }
        public FileImportTypeEnum FileType { get; set; }
        public ImportStatusEnum Status { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
    }
}
