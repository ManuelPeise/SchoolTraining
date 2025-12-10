using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IFamilyAdministrationService
    {
        Task<FileDownloadModel?> DownloadFamilyImportTemplate();
        Task UploadFamilyTemplateFile(FileUploadModel model);
        Task<List<FamilyModel>> GetFamilies();
        Task UpdateFamilies(List<FamilyModel> families);
    }
}
