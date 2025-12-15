using Shared.Models;
using Shared.Models.Import;

namespace Logic.Import.FileImport.Interfaces
{
    public interface IFileImporter
    {
        Task<List<ImportFileModel>> GetFiles();
        Task<NotificationDataResponse<List<ImportFileModel>>> ImportFile(int fileId);
        Task ImportFiles();
        Task<List<ImportFileModel>> DeleteFile(int fileId);
        Task<List<ImportFileModel>> DeleteFiles();
    }
}
