using Microsoft.AspNetCore.Http;
using Shared.Models;
using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IFamilyAdministrationService
    {
        Task<bool> ImportFile(FormFile file);
        Task<FileResponse?> DownloadFamilyImportTemplate();
        Task<bool> UploadFamilyTemplateFile(FormFile file);
        Task<List<FamilyModel>> GetFamilies();
        Task<List<FamilyModel>> UpdateFamilies(List<FamilyModel> families);
    }
}
