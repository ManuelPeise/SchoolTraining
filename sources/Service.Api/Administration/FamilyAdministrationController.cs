using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration;

namespace Service.Api.Administration
{
    [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
    public class FamilyAdministrationController : ApiControllerBase
    {
        private readonly IFamilyAdministrationService _familyAdministrationService;

        public FamilyAdministrationController(IFamilyAdministrationService familyAdministrationService)
        {
            _familyAdministrationService = familyAdministrationService;
        }

        [HttpGet(Name = "GetFamilies")]
        public async Task<List<FamilyModel>> GetFamilies() => await _familyAdministrationService.GetFamilies();

        [HttpGet(Name = "DownloadFamilyImportTemplate")]
        public async Task<FileDownloadModel?> DownloadFamilyImportTemplate()
        {
            return await _familyAdministrationService.DownloadFamilyImportTemplate();
        }

        [HttpPost(Name = "UploadFamilyTemplateFile")]
        public async Task UploadFamilyTemplateFile([FromBody] FileUploadModel model)
        {
            await _familyAdministrationService.UploadFamilyTemplateFile(model);
        }
    }
}
