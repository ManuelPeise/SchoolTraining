using Data.Entities;
using Logic.Administration.Extensions;
using Logic.Database;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Import;
using System.Text.Json;

namespace Logic.Administration.FileImport
{
    public class FamilyFileImport : AFileImport
    {
        public FamilyFileImport(IHttpContextAccessor httpContextAccessor, IDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory) { }

        protected override async Task<int> Import(FileStream fileStream)
        {
            using (var unitOfWork = new UnitOfWork(DbContextFactory, DbContextTypeEnum.MySql))
            {
                string fileContent;

                try
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        fileContent = await reader.ReadToEndAsync();
                    }

                    var familyImportModel = JsonSerializer.Deserialize<FamilyImportModel>(fileContent);

                    var familyEntity = familyImportModel?.ToImportEntity();

                    if (familyImportModel == null || familyEntity == null)
                    {
                        await SaveImportFile(unitOfWork, FileImportTypeEnum.Family, fileContent, ImportStatusEnum.Failed);

                        return await unitOfWork.LogMessage(new LogMessageEntity
                        {
                            Message = "Could not import family, please check import file.",
                            ExeptionMessage = string.Empty,
                            StackTrace = string.Empty,
                            LogLevel = LogLevelEnum.Error
                        }, true);
                    }

                    await unitOfWork.FamilyRepository.AddAsync(familyEntity);

                    await SaveImportFile(unitOfWork, FileImportTypeEnum.Family, fileContent, ImportStatusEnum.Success);

                    return await unitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "Family file import success",
                        ExeptionMessage = string.Empty,
                        StackTrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, true);
                }
                catch (Exception exception)
                {
                    return await unitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "Exception occurred during family import.",
                        ExeptionMessage = exception.Message,
                        StackTrace = exception?.StackTrace ?? string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, true);
                }
            }
        }

        protected override async Task<Stream?> GetFile(FileImportTypeEnum fileType)
        {
            return await Task.FromResult(GetFileTemplate(fileType));
        }
    }
}
