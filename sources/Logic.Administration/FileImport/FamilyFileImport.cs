using Data.Entities;
using Logic.Administration.Extensions;
using Logic.Database;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Administration;
using Shared.Models.Import;
using System.Text.Json;

namespace Logic.Administration.FileImport
{
    public class FamilyFileImport : AFileImport
    {
        public FamilyFileImport(IHttpContextAccessor httpContextAccessor, IDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory) { }

        public override async Task<int> Import(string fileContent, string fileName)
        {
            using (var unitOfWork = new UnitOfWork(DbContextFactory, DbContextTypeEnum.MySql))
            {
                try
                {
                  
                    var familyImportModel = JsonSerializer.Deserialize<FamilyImportModel>(fileContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                    });

                    var familyEntity = familyImportModel?.ToImportEntity();

                    if (familyImportModel == null || familyEntity == null)
                    {
                        await SaveImportFile(unitOfWork, FileImportTypeEnum.Family, fileName, fileContent, ImportStatusEnum.Failed);

                        return await unitOfWork.LogMessage(new LogMessageEntity
                        {
                            Message = "Could not import family, please check import file.",
                            ExeptionMessage = string.Empty,
                            StackTrace = string.Empty,
                            LogLevel = LogLevelEnum.Error
                        }, true);
                    }

                    await unitOfWork.FamilyRepository.AddAsync(familyEntity);

                    await SaveImportFile(unitOfWork, FileImportTypeEnum.Family, fileName, fileContent, ImportStatusEnum.Success);

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

        public override async Task<FileDownloadModel> GetFile(FileImportTypeEnum fileType)
        {
            var result = await Task.FromResult(GetFileTemplate(fileType));

            return new FileDownloadModel
            {
                FileName = result.fileName,
                FileContent = result.base64String
            };
        }
    }
}
