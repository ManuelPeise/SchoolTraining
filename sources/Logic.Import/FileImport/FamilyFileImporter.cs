using Data.Entities;
using Logic.Import.FileImport.Extensions;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Import;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logic.Import.FileImport
{
    public class FamilyFileImporter : AFileImport
    {

        public FamilyFileImporter(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork) : base(httpContextAccessor, unitOfWork) { }

        public override async Task<bool> ImportFile(string fileContent, string fileName)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.Converters.Add(new FlexibleDateTimeConverter());

                var familyImportModel = JsonSerializer.Deserialize<FamilyImportModel>(fileContent, options);
                if (familyImportModel == null)
                {
                    await LogError("Deserialization of FamilyImportModel failed.");
                    return false;
                }

                var familyEntity = familyImportModel.ToImportEntity();

                var existingFamilyEntity = await UnitOfWork.FamilyRepository
                    .GetByAsync(x => x.ContactMailAddress == familyEntity.ContactMailAddress &&
                                   x.Name == familyEntity.Name, true, IncludeExpressions.IncludeFamilyMembers);

                if (existingFamilyEntity == null)
                {
                    await UnitOfWork.FamilyRepository.AddAsync(familyEntity);
                    await LogInfo("New family added!");
                }
                else
                {
                    UpdateFamilyEntity(existingFamilyEntity, familyEntity);
                    await LogInfo("Family updated!");
                }

                await UnitOfWork.SaveChangesAsync(CurrentUser.UserName);
                await LogInfo("Family file import success, import file saved!");

                return true;
            }
            catch (Exception exception)
            {
                await LogError($"One or more errors occurred during family import: {exception.Message}", exception);

                return false;
            }
        }

        private void UpdateFamilyEntity(FamilyEntity existing, FamilyEntity updated)
        {
            existing.Name = updated.Name;
            existing.ContactMailAddress = updated.ContactMailAddress;
            existing.IsActive = updated.IsActive;

            foreach (var member in updated.Users)
            {
                var existingMember = existing.Users
                    .FirstOrDefault(x => x.FirstName.ToLowerInvariant() == member.FirstName.ToLowerInvariant() &&
                                         x.LastName.ToLowerInvariant() == member.LastName.ToLowerInvariant() &&
                                         x.DateOfBirth == member.DateOfBirth);

                if (existingMember == null)
                {
                    existing.Users.Add(member);
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
                    existingMember.Credentials = member.Credentials;
                    existingMember.Settings = member.Settings;
                }
            }
            UnitOfWork.FamilyRepository.Update(existing);
        }

        private async Task LogInfo(string message)
        {
            await UnitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = string.Empty,
                StackTrace = string.Empty,
                Module = nameof(FamilyFileImporter),
                LogLevel = LogLevelEnum.Info
            }, false);
        }

        private async Task LogError(string message, Exception? exception = null)
        {
            await UnitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = exception?.Message ?? string.Empty,
                StackTrace = exception?.StackTrace ?? string.Empty,
                Module = nameof(FamilyFileImporter),
                LogLevel = LogLevelEnum.Error
            }, true);
        }
    }
}
