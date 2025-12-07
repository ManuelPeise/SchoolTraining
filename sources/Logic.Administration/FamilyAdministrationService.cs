using Data.Entities;
using Logic.Administration.Interfaces;
using Logic.Database;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Administration.Interfaces;
using Logic.Administration.Extensions;
using Shared.Models.Administration;

namespace Logic.Administration
{
    public class FamilyAdministrationService : LogicBase, IFamilyAdministrationService
    {
        private readonly IFileImportFactory _fileImportFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbContextFactory _dbContextFactory;

        public FamilyAdministrationService(
            IFileImportFactory fileImportFactory,
            IHttpContextAccessor httpContextAccessor,
            IDbContextFactory dbContextFaytory) : base(httpContextAccessor)
        {
            _fileImportFactory = fileImportFactory;
            _httpContextAccessor = httpContextAccessor;
            _dbContextFactory = dbContextFaytory;
        }

        public async Task UploadFamilyTemplateFile(FileUploadModel model)
        {
            using (var unitOfWork = new UnitOfWork(_dbContextFactory, DbContextTypeEnum.MySql))
            {
                try
                {
                    var file = model.File;

                    var importer = _fileImportFactory.GetFileImport(FileImportTypeEnum.Family, _httpContextAccessor, _dbContextFactory);

                    if (importer == null || file == null)
                    {
                        return;
                    }

                    using (var stream = file.OpenReadStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var content = await reader.ReadToEndAsync();

                        await importer.Import(content, file.Name);
                    }
                }
                catch (Exception exception)
                {
                    await unitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "Import family import template file failed.",
                        ExeptionMessage = exception.Message,
                        StackTrace = exception?.StackTrace ?? string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, true);
                }
            }
        }

        public async Task<FileDownloadModel?> DownloadFamilyImportTemplate()
        {
            using (var unitOfWork = new UnitOfWork(_dbContextFactory, DbContextTypeEnum.MySql))
            {
                try
                {
                    var importer = _fileImportFactory.GetFileImport(FileImportTypeEnum.Family, _httpContextAccessor, _dbContextFactory);

                    if (importer == null)
                    {
                        return null;
                    }

                    return await importer.GetFile(FileImportTypeEnum.Family);
                }
                catch (Exception exception)
                {
                    await unitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "Downloading family import template file failed.",
                        ExeptionMessage = exception.Message,
                        StackTrace = exception?.StackTrace ?? string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, true);

                    return null;
                }
            }
        }

        public async Task<List<FamilyModel>> GetFamilies()
        {
            using (var unitOfWork = new UnitOfWork(_dbContextFactory, DbContextTypeEnum.MySql))
            {
                try
                {
                    var familyEntities = await unitOfWork.FamilyRepository.GetAllAsync(true, IncludeExpressions.IncludeFamilyMembers);

                    if (familyEntities == null || !familyEntities.Any())
                    {
                        return new();
                    }

                    var familyCollection = familyEntities.ToFamilyList();

                    if (CurrentUser == null)
                    {
                        throw new Exception("");
                    }

                    if (CurrentUser.UserRole == UserRoleEnum.Admin)
                    {

                        return familyCollection
                            .Where(family => family.FamilyId == CurrentUser.FamilyId)
                            .ToList();
                    }

                    return familyCollection;
                }
                catch (Exception exception)
                {
                    await unitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "Could not load families from database.",
                        ExeptionMessage = exception.Message,
                        StackTrace = exception?.StackTrace ?? string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, true);

                    return new();
                }
            }
        }
    }
}
