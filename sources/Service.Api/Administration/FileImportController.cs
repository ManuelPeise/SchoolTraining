using Logic.Import.FileImport.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Service.Api.Scheduler;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Import;

namespace Service.Api.Administration
{
    public class FileImportController: ApiControllerBase
    {
        private readonly IFileImporter _fileImporter;

        public FileImportController(IFileImporter fileImporter)
        {
            _fileImporter = fileImporter;
        }

        [HttpGet(Name = "GetFiles")]
        [JwtAuth(AllowSystemAdmin = true)]
        public async Task<List<ImportFileModel>> GetFiles()
        {
            return  await _fileImporter.GetFiles();
        }

        [HttpPost(Name = "ImportFile")]
        [JwtAuth(AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<ImportFileModel>>> ImportFile([FromQuery]int id)
        {
            return await _fileImporter.ImportFile(id);
        }

        [HttpPost(Name = "DeleteFile")]
        [JwtAuth(AllowSystemAdmin = true)]
        public async Task<List<ImportFileModel>> DeleteFile([FromQuery] int id)
        {
           return await _fileImporter.DeleteFile(id);
        }

        [HttpPost(Name = "DeleteFiles")]
        [JwtAuth(AllowSystemAdmin = true)]
        public async Task<List<ImportFileModel>> DeleteFiles()
        {
            return await _fileImporter.DeleteFiles();
        }

        /// <summary>
        /// Called by scheduler to process pending import files.
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "ImportFiles")]
        [SchedulerAuthorize]
        public async Task<IActionResult> ImportFiles()
        {
            await _fileImporter.ImportFiles();

            return Ok();
        }
    }
}
