using Logic.Import.FileImport.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Logic.Import.FileImport
{
    public class FileImporter : LogicBase, IFileImporter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileImporterFactory _fileImporterFactory;
        public FileImporter(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IFileImporterFactory fileImporterFactory) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _fileImporterFactory = fileImporterFactory;
        }

        public async Task ImportFiles()
        {
            try
            {
                var databaseChanged = false;

                var importFilesToProcess = await _unitOfWork.ImportFileRepository.GetAllByAsync(x => x.Status == ImportStatusEnum.Pending, true);

                if (importFilesToProcess.Any())
                {
                    foreach (var importFile in importFilesToProcess)
                    {
                        try
                        {
                            var importer = _fileImporterFactory.GetFileImporter(importFile.FileType, HttpContextAccessor, _unitOfWork);

                            if (importer != null)
                            {
                                using (var stream = new MemoryStream(importFile.FileContent))
                                using (var reader = new StreamReader(stream))
                                {
                                    var fileContent = await reader.ReadToEndAsync();

                                    await importer.ImportFile(fileContent, importFile.FileName);

                                    importFile.Status = ImportStatusEnum.Success;

                                    _unitOfWork.ImportFileRepository.Update(importFile);

                                    databaseChanged = true;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            importFile.Status = ImportStatusEnum.Failed;

                            _unitOfWork.ImportFileRepository.Update(importFile);

                            databaseChanged = true;

                            await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                            {
                                Message = $"No importer found for file type: {importFile.FileType}.",
                                ExeptionMessage = exception.Message,
                                StackTrace = exception.StackTrace,
                                LogLevel = LogLevelEnum.Error,
                                Module = nameof(FileImporter),
                            });
                        }
                    }
                }

                if(databaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                {
                    Message = "Exception occurred during file import.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(FileImporter),
                }, true);

            }
        }
    }
}
