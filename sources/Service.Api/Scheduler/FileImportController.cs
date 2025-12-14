using Logic.Import.FileImport.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Service.Api.Scheduler;

namespace Service.Api.Scheduler
{
    public class FileImportController: ApiControllerBase
    {
        private readonly IFileImporter _fileImporter;

        public FileImportController(IFileImporter fileImporter)
        {
            _fileImporter = fileImporter;
        }

        [HttpPost("ImportFiles")]
        [SchedulerAuthorize]
        public async Task<IActionResult> ImportFiles()
        {
            await _fileImporter.ImportFiles();

            return Ok();
        }
    }
}
