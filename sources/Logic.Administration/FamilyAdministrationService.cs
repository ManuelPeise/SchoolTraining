using Data.Entities;
using Logic.Administration.Extensions;
using Logic.Administration.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Administration;
using System.Diagnostics;

namespace Logic.Administration
{
    public class FamilyAdministrationService : LogicBase, IFamilyAdministrationService
    {
        private readonly IFileImportFactory _fileImportFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitIOfWork;

        public FamilyAdministrationService(
            IFileImportFactory fileImportFactory,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            _fileImportFactory = fileImportFactory;
            _httpContextAccessor = httpContextAccessor;
            _unitIOfWork = unitOfWork;
        }

        public async Task<bool> ImportFile(FormFile file)
        {
            try
            {
                var importer = _fileImportFactory.GetFileImport(FileImportTypeEnum.Family, _httpContextAccessor, _unitIOfWork);

                if (importer == null)
                {
                    return false;
                }

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = await reader.ReadToEndAsync();
                    await importer.SaveImportFile(FileImportTypeEnum.Family, file.FileName, fileContent, ImportStatusEnum.Pending);
                }

                return true;
            }
            catch (Exception exception)
            {
                await _unitIOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Deleting all families failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, true);

                return false;
            }

        }

        public async Task<bool> UploadFamilyTemplateFile(FormFile file)
        {
            try
            {
                var importer = _fileImportFactory.GetFileImport(FileImportTypeEnum.Family, _httpContextAccessor, _unitIOfWork);

                if (importer == null)
                {
                    return false;
                }

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = await reader.ReadToEndAsync();
                    await importer.Import(fileContent, file.FileName);
                }

                return true;

            }
            catch (Exception exception)
            {
                await _unitIOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Import family import template file failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, true);

                return false;
            }

        }

        public async Task<FileResponse?> DownloadFamilyImportTemplate()
        {
            try
            {
                var importer = _fileImportFactory.GetFileImport(FileImportTypeEnum.Family, _httpContextAccessor, _unitIOfWork);

                if (importer == null)
                {
                    return null;
                }

                return await importer.GetFile(FileImportTypeEnum.Family);
            }
            catch (Exception exception)
            {
                await _unitIOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Downloading family import template file failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, true);

                return null;
            }
        }

        public async Task<List<FamilyModel>> GetFamilies()
        {
            try
            {
                var familyEntities = await _unitIOfWork.FamilyRepository.GetAllAsync(true, IncludeExpressions.IncludeFamilyMembers);

                if (familyEntities == null || !familyEntities.Any())
                {
                    return new();
                }

                var familyCollection = familyEntities.ToFamilyList();

                return familyCollection;

            }
            catch (Exception exception)
            {
                await _unitIOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Could not load families from database.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, true);

                return new();
            }
        }

        public async Task<List<FamilyModel>> UpdateFamilies(List<FamilyModel> families)
        {
            var databaseChanged = false;

            try
            {
                foreach (var family in families)
                {
                    var familyEntity = await _unitIOfWork.FamilyRepository.GetByIdAsync(family.FamilyId, true);

                    if (familyEntity == null)
                    {
                        Debug.WriteLine($"Family with ID {family.FamilyId} not found.");
                        continue;
                    }

                    familyEntity.IsActive = family.IsActive;

                    _unitIOfWork.FamilyRepository.Update(familyEntity);

                    databaseChanged = true;
                }

                if (databaseChanged)
                {
                    await _unitIOfWork.SaveChangesAsync(CurrentUser.UserName);
                }
            }
            catch (Exception exception)
            {
                await _unitIOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Could not update families in database.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, true);
            }

            return await GetFamilies();
        }
    }
}
