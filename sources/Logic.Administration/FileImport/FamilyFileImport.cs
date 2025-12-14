using Data.Entities;
using Logic.Administration.Extensions;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Import;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logic.Administration.FileImport
{
    public class FamilyFileImport : AFileImport
    {
        public FamilyFileImport(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) : base(httpContextAccessor, unitOfWork) { }

        public override async Task<int> Import(string fileContent, string fileName)
        {
            try
            {
                FamilyEntity? familyEntity;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip
                };

                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

                var familyImportModel = JsonSerializer.Deserialize<FamilyImportModel>(fileContent, options);

                if (familyImportModel == null)
                {
                    await SaveImportFile(FileImportTypeEnum.Family, fileName, fileContent, ImportStatusEnum.Failed);

                    return await UnitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "Exception occurred during family import.",
                        ExeptionMessage = string.Empty,
                        StackTrace = string.Empty,
                        Module = nameof(FamilyFileImport),
                        LogLevel = LogLevelEnum.Error
                    }, true);

                }

                familyEntity = familyImportModel.ToImportEntity();

                var existingFamilyEntity = await UnitOfWork.FamilyRepository
                    .GetByAsync(x => x.ContactMailAddress == familyEntity.ContactMailAddress &&
                    x.Name == familyEntity.Name, true, IncludeExpressions.IncludeFamilyMembers);

                if (existingFamilyEntity == null)
                {
                    await UnitOfWork.FamilyRepository.AddAsync(familyEntity);

                    await UnitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "New family added!",
                        ExeptionMessage = string.Empty,
                        StackTrace = string.Empty,
                        Module = nameof(FamilyFileImport),
                        LogLevel = LogLevelEnum.Info
                    }, false);
                }
                else
                {
                    existingFamilyEntity.Name = familyEntity.Name;
                    existingFamilyEntity.ContactMailAddress = familyEntity.ContactMailAddress;
                    existingFamilyEntity.IsActive = familyEntity.IsActive;

                    foreach (var member in familyEntity.Users)
                    {
                        var existingMember = existingFamilyEntity.Users
                            .FirstOrDefault(x => ToLowerCase(x.FirstName) == ToLowerCase(member.FirstName) &&
                            ToLowerCase(x.LastName) == ToLowerCase(member.LastName) &&
                            x.DateOfBirth == member.DateOfBirth);

                        if (existingMember == null)
                        {
                            existingFamilyEntity.Users.Add(member);

                            await UnitOfWork.LogMessage(new LogMessageEntity
                            {
                                Message = "New family member added.",
                                ExeptionMessage = string.Empty,
                                StackTrace = string.Empty,
                                Module = nameof(FamilyFileImport),
                                LogLevel = LogLevelEnum.Info
                            }, false);
                        }
                        else
                        {
                            existingMember.FirstName = member.FirstName;
                            existingMember.LastName = member.LastName;
                            existingMember.Username = member.Username;
                            existingMember.Email = member.Email;
                            existingMember.UserRole = member.UserRole;
                            existingMember.DateOfBirth = member.DateOfBirth;
                            existingMember.IsActive = member.IsActive;

                            await UnitOfWork.LogMessage(new LogMessageEntity
                            {
                                Message = "Family member updated.",
                                ExeptionMessage = string.Empty,
                                StackTrace = string.Empty,
                                Module = nameof(FamilyFileImport),
                                LogLevel = LogLevelEnum.Info
                            }, false);
                        }

                        UnitOfWork.FamilyRepository.Update(existingFamilyEntity);
                    }

                }

                await SaveImportFile(FileImportTypeEnum.Family, fileName, fileContent, ImportStatusEnum.Success);

                await UnitOfWork.SaveChangesAsync(CurrentUser.UserName);

                return await UnitOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Family file import success, import file saved!",
                    ExeptionMessage = string.Empty,
                    StackTrace = string.Empty,
                    Module = nameof(FamilyFileImport),
                    LogLevel = LogLevelEnum.Info
                }, true);



            }
            catch (Exception exception)
            {
                return await UnitOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "One or more errors occurred during family import.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    Module = nameof(FamilyFileImport),
                    LogLevel = LogLevelEnum.Error
                }, true);
            }
        }

        public override async Task<FileResponse> GetFile(FileImportTypeEnum fileType)
        {
            var result = await Task.FromResult(GetFileTemplate(fileType));

            var bytes = Convert.FromBase64String(result.base64String) ?? new byte[0];

            if (bytes == null || bytes.Length == 0)
            {
                throw new Exception("File content is empty.");
            }

            return new FileResponse
            {
                ContentType = "application/octet-stream",
                FileName = result.fileName ?? "template-file",
                Bytes = bytes.ToList()
            };
        }

        private string ToLowerCase(string input)
        {
            return input.ToLowerInvariant();
        }
    }
}
