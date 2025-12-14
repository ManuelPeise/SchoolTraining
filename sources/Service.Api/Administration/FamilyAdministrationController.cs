using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Administration;
using System.Diagnostics;

namespace Service.Api.Administration
{
    [JwtAuth(AllowSystemAdmin = true)]
    public class FamilyAdministrationController : ApiControllerBase
    {
        private readonly IFamilyAdministrationService _familyAdministrationService;

        public FamilyAdministrationController(IFamilyAdministrationService familyAdministrationService)
        {
            _familyAdministrationService = familyAdministrationService;
        }

        [HttpGet(Name = "GetFamilies")]
        public async Task<List<FamilyModel>> GetFamilies()
        {
            return await _familyAdministrationService.GetFamilies();
        }

        [HttpPost(Name = "UpdateFamilies")]
        public async Task<List<FamilyModel>> UpdateFamilies([FromBody] List<FamilyModel> families)
        {
           return await _familyAdministrationService.UpdateFamilies(families);
        }

        [HttpGet(Name = "DownloadFamilyImportTemplate")]
        public async Task<IActionResult> DownloadFamilyImportTemplate()
        {
            var result = await _familyAdministrationService.DownloadFamilyImportTemplate();

            if(result == null)
            {
                return NotFound();
            }

            Debug.WriteLine($"DownloadFamilyImportTemplate: FileName={result.FileName}, ContentType={result.ContentType}, Bytes={result.Bytes.Count}");

            var file = File(result.Bytes.ToArray(), result.ContentType, result.FileName);
            

            return file;
        }

        [HttpPost(Name = "UploadFamilyTemplateFile")]
        public async Task<NotificationResponse> UploadFamilyTemplateFile([FromForm] FormFile file)
        {
            if (file == null)
            {
                return new NotificationResponse
                {
                    Success = false,
                    ResourceKey = "common.notificationFamilyImportFailed",
                };
            }

            var importResult = await _familyAdministrationService.UploadFamilyTemplateFile(file);

            return new NotificationResponse
            {
                Success = importResult,
                ResourceKey = "common.notificationFamilyImportSuccess",
            };
        }
    }
}
