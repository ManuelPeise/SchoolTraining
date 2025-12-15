using Data.Entities;
using Logic.Import.FileImport.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Import;
using System.Globalization;

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

        public async Task<List<ImportFileModel>> GetFiles()
        {
            try
            {
                var files = await _unitOfWork.ImportFileRepository.GetAllAsync(true);

                return files.Select(x => new ImportFileModel
                {
                    FileId = x.Id,
                    FileName = x.FileName,
                    FileDate = x.FileDate,
                    FileType = x.FileType,
                    Status = x.Status,
                    LastUpdate = GetLastUpdateAt(x.UpdatedAt, x.CreatedAt)
                        .ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                    LastUpdateBy = GetLastUpdateBy(x.UpdatedBy, x.CreatedBy),
                }).ToList();
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                {
                    Message = "Exception occurred during loading import files from database.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(FileImporter),
                }, true, CurrentUser.FamilyId);
            }

            return new List<ImportFileModel>();
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

                                    var result = await importer.ImportFile(fileContent, importFile.FileName);

                                    if (!result)
                                    {
                                        await _unitOfWork.LogMessage(new LogMessageEntity
                                        {
                                            Message = $"Import for file type: {importFile.FileType} did not complete successfully.",
                                            LogLevel = LogLevelEnum.Error,
                                            Module = nameof(FileImporter),
                                            TimeStamp = DateTime.UtcNow,
                                        }, false, CurrentUser.FamilyId);

                                        continue;
                                    }

                                    importFile.Status = ImportStatusEnum.Success;

                                    _unitOfWork.ImportFileRepository.Update(importFile);

                                    await _unitOfWork.LogMessage(new LogMessageEntity
                                    {
                                        Message = $"Import for file type: {importFile.FileType} completed successfully.",
                                        LogLevel = LogLevelEnum.Info,
                                        Module = nameof(FileImporter),
                                        TimeStamp = DateTime.UtcNow,
                                    }, false, CurrentUser.FamilyId);

                                    databaseChanged = true;

                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            importFile.Status = ImportStatusEnum.Failed;

                            _unitOfWork.ImportFileRepository.Update(importFile);

                            databaseChanged = true;

                            await _unitOfWork.LogMessage(new LogMessageEntity
                            {
                                Message = $"Import for file type: {importFile.FileType} failed.",
                                ExeptionMessage = exception.Message,
                                StackTrace = exception.StackTrace,
                                LogLevel = LogLevelEnum.Error,
                                Module = nameof(FileImporter),
                                TimeStamp = DateTime.UtcNow,
                            }, false, CurrentUser.FamilyId);
                        }
                    }
                }

                if (databaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                {
                    Message = "Exception occurred during importing files.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(FileImporter),
                    TimeStamp = DateTime.UtcNow,
                }, true, CurrentUser.FamilyId);

            }
        }

        public async Task<NotificationDataResponse<List<ImportFileModel>>> ImportFile(int fileId)
        {
            try
            {
                var databaseChanged = false;

                var importFileToProcess = await _unitOfWork.ImportFileRepository.GetByIdAsync(fileId, true);

                if (importFileToProcess != null)
                {
                    try
                    {
                        var importer = _fileImporterFactory.GetFileImporter(importFileToProcess.FileType, HttpContextAccessor, _unitOfWork);

                        if (importer != null)
                        {
                            using (var stream = new MemoryStream(importFileToProcess.FileContent))
                            using (var reader = new StreamReader(stream))
                            {
                                var fileContent = await reader.ReadToEndAsync();

                                await importer.ImportFile(fileContent, importFileToProcess.FileName);

                                importFileToProcess.Status = ImportStatusEnum.Success;

                                _unitOfWork.ImportFileRepository.Update(importFileToProcess);

                                databaseChanged = true;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        importFileToProcess.Status = ImportStatusEnum.Failed;

                        _unitOfWork.ImportFileRepository.Update(importFileToProcess);

                        databaseChanged = true;

                        await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                        {
                            Message = $"Import for file type: {importFileToProcess.FileType} failed.",
                            ExeptionMessage = exception.Message,
                            StackTrace = exception.StackTrace,
                            LogLevel = LogLevelEnum.Error,
                            Module = nameof(FileImporter),
                            TimeStamp = DateTime.UtcNow,
                        }, true, CurrentUser.FamilyId);
                    }
                }

                if (databaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return new NotificationDataResponse<List<ImportFileModel>>
                {
                    Success = true,
                    ResourceKey = "notificationFileImportSuccess",
                    Data = await GetFiles(), 
                };
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                {
                    Message = "fileImportFailed",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(FileImporter),
                    TimeStamp = DateTime.UtcNow,
                }, true, CurrentUser.FamilyId);

                return new NotificationDataResponse<List<ImportFileModel>>
                {
                    Success = false,
                    ResourceKey = "notificationFileImportFailed",
                    Data = await GetFiles(),
                };
            }
        }

        public async Task<List<ImportFileModel>> DeleteFile(int fileId)
        {
            try
            {
                var importFileToDelete = await _unitOfWork.ImportFileRepository.GetByIdAsync(fileId, true);
                if (importFileToDelete != null)
                {
                    _unitOfWork.ImportFileRepository.Remove(importFileToDelete);
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                {
                    Message = "Exception occurred during deleting import file.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(FileImporter),
                    TimeStamp = DateTime.UtcNow,
                }, true, CurrentUser.FamilyId);
            }

            return await GetFiles();
        }

        public async Task<List<ImportFileModel>> DeleteFiles()
        {
            try
            {
                var importFilesToDelete = await _unitOfWork.ImportFileRepository.GetAllByAsync(
                    x => x.Status == ImportStatusEnum.Success || x.Status == ImportStatusEnum.Failed, true);

                if (importFilesToDelete.Any())
                {
                    foreach (var importFileToDelete in importFilesToDelete)
                    {
                        _unitOfWork.ImportFileRepository.Remove(importFileToDelete);
                    }

                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                {
                    Message = "Exception occurred during deleting import file.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(FileImporter),
                    TimeStamp = DateTime.UtcNow,
                }, true, CurrentUser.FamilyId);
            }

            return await GetFiles();
        }

        private DateTime GetLastUpdateAt(DateTime? updetedAt, DateTime createdAt)
        {
            return updetedAt == null || updetedAt == DateTime.MinValue ? createdAt : updetedAt.Value;
        }

        private string GetLastUpdateBy(string? updetedAt, string createdAt)
        {
            return string.IsNullOrEmpty(updetedAt) ? createdAt : updetedAt;
        }
    }
}
