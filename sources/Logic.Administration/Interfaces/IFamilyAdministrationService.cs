using Microsoft.AspNetCore.Http;
using Shared.Models;
using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IFamilyAdministrationService
    {
        Task<List<FamilyModel>> GetFamilies();
        Task<FileResponse?> DownloadFamilyImportTemplate();
        Task<bool> UploadFamilyTemplateFile(IFormFile file);
        Task<List<FamilyModel>> UpdateFamilies(List<FamilyModel> families);
    }
}
