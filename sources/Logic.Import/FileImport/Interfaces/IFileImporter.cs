using Shared.Models.Import;

namespace Logic.Import.FileImport.Interfaces
{
    public interface IFileImporter
    {
        Task<List<ImportFileModel>> GetFiles();
        Task<List<ImportFileModel>> DeleteFile(int fileId);
        Task<List<ImportFileModel>> DeleteFiles();
        Task ImportFiles();
        Task<List<ImportFileModel>> ImportFile(int fileId);
    }
}
