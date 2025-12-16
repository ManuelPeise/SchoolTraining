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

        public override async Task<bool> ImportFile(string fileContent, string fileName, ImportFileEntity fileEntity)
        {
            try
            {
                var isDatabaseChanged = false;
                var (isValidFileName, fileDate) = ValidateFileName(fileName);

                if (!isValidFileName || fileDate == null)
                {
                    fileEntity.Status = ImportStatusEnum.Failed;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await LogError($"Family import from file '{fileName}' failed due to invalid file name format.");

                    return false;
                }

                var options = GetSerializerOptions(includeDateOptions: true);

                var familyImportModel = JsonSerializer.Deserialize<FamilyImportModel>(fileContent, options);

                if (familyImportModel == null || familyImportModel.Family == null)
                {
                    fileEntity.Status = ImportStatusEnum.Failed;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await LogError($"Family import from file '{fileName}' failed due to invalid file content.");

                    return false;
                }

                if (!ValidateFamilyImportModel(familyImportModel))
                {
                    fileEntity.Status = ImportStatusEnum.Failed;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await LogError($"Family import from file '{fileName}' failed due to validation errors in the import model.");

                    return false;
                }

                var newEntity = familyImportModel.Family.ToImportEntity();

                var existingFamilyEntity = await UnitOfWork.FamilyRepository
                    .GetByAsync(x => x.IdExternal == newEntity.IdExternal, true, IncludeExpressions.IncludeFamilyMembers);

                var isNew = existingFamilyEntity == null;

                if (isNew)
                {
                    await UnitOfWork.FamilyRepository.AddAsync(newEntity);
                    await LogInfo($"Family [{newEntity.IdExternal}] added!");

                    isDatabaseChanged = true;
                }
                else
                {
                    if (existingFamilyEntity == null) { return false; }

                    UpdateFamilyEntity(existingFamilyEntity, newEntity);

                    await LogInfo($"Family [{existingFamilyEntity.IdExternal}] updated!");

                    isDatabaseChanged = true;
                }

                if (isDatabaseChanged)
                {
                    await LogInfo($"Family import from file '{fileName}' completed successfully.");

                    // Save import file record
                    fileEntity.Status = ImportStatusEnum.Success;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await UnitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return true;
            }
            catch (Exception exception)
            {
                await LogError($"One or more errors occurred during family import: {exception.Message}", exception);

                return false;
            }
        }

        // expecting file name format: Family_import_Name_YYYYMMDDHHMMSS.json
        private (bool isValid, DateTime? fileDate) ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return (false, null);
            }

            if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return (false, null);
            }

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            var fileNameParts = fileNameWithoutExtension.Split('_');

            if (fileNameParts.Length != 3 || !fileNameParts[0].Equals("Familyimport", StringComparison.OrdinalIgnoreCase))
            {
                return (false, null);
            }

            if (!DateTime.TryParseExact(fileNameParts[2], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var fileDate))
            {
                return (false, null);
            }

            return (true, fileDate);
        }

        private bool ValidateFamilyImportModel(FamilyImportModel? importModel)
        {
            if (importModel == null)
            {
                return false;
            }

            var existingExternalIds = importModel.ExistingExternalIds
                .Select(f => new { Id = f })
                .GroupBy(x => x.Id)
                .ToList();

            if (existingExternalIds.Any(g => g.Count() > 1))
            {
                return false;
            }

            return true;
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
                LogLevel = LogLevelEnum.Info,
                TimeStamp = DateTime.UtcNow,
            }, false, CurrentUser.FamilyId);
        }

        private async Task LogError(string message, Exception? exception = null)
        {
            await UnitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = exception?.Message ?? string.Empty,
                StackTrace = exception?.StackTrace ?? string.Empty,
                Module = nameof(FamilyFileImporter),
                LogLevel = LogLevelEnum.Error,
                TimeStamp = DateTime.UtcNow,
            }, true, CurrentUser.FamilyId);
        }
    }
}
