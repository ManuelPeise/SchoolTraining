using Data.Entities;
using Logic.Administration.Extensions;
using Logic.Administration.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Import;
using System.Diagnostics;
using System.Text;


namespace Logic.Administration
{
    public class FamilyAdministrationService : LogicBase, IFamilyAdministrationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitIOfWork;

        public FamilyAdministrationService(

            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitIOfWork = unitOfWork;
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
                    TimeStamp = DateTime.UtcNow,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(FamilyAdministrationService),
                }, true, CurrentUser.FamilyId);

                return new();
            }
        }

        public async Task<bool> UploadFamilyTemplateFile(IFormFile file)
        {
            try
            {
                var fileNameParts = Path.GetFileNameWithoutExtension(file.FileName).Split('_');

                if (DateTime.TryParseExact(fileNameParts.Last(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime fileDate))
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var fileContent = await reader.ReadToEndAsync();

                        var bytes = Encoding.UTF8.GetBytes(fileContent);

                        var fileImportEntity = new ImportFileEntity
                        {
                            FileName = file.FileName,
                            FileContent = bytes,
                            FileType = FileImportTypeEnum.Family,
                            Status = ImportStatusEnum.Pending,
                            FileDate = fileDate,
                        };

                        await _unitIOfWork.ImportFileRepository.AddAsync(fileImportEntity);

                        await _unitIOfWork.SaveChangesAsync(CurrentUser.UserName);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception exception)
            {
                await _unitIOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Import family import template file failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    Module = nameof(FamilyAdministrationService),
                    LogLevel = LogLevelEnum.Error,
                    TimeStamp = DateTime.UtcNow,
                }, true, CurrentUser.FamilyId);

                return false;
            }

        }

        public async Task<FileResponse?> DownloadFamilyImportTemplate()
        {
            try
            {
                return await GetFile(FileImportTypeEnum.Family);
            }
            catch (Exception exception)
            {
                await _unitIOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Downloading family import template file failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error,
                    TimeStamp = DateTime.UtcNow,
                }, true, CurrentUser.FamilyId);

                return null;
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
                    Module = nameof(FamilyAdministrationService),
                    LogLevel = LogLevelEnum.Error,
                    TimeStamp = DateTime.UtcNow,
                }, true, CurrentUser.FamilyId);
            }

            return await GetFamilies();
        }

        private async Task<FileResponse> GetFile(FileImportTypeEnum fileType)
        {
            var result = await GetFileTemplate(fileType);

            var bytes = !string.IsNullOrEmpty(result.base64String) ? Convert.FromBase64String(result.base64String) : new byte[0];

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

        private async Task<(string? fileName, string? base64String)> GetFileTemplate(FileImportTypeEnum fileType)
        {
            var familyEntities = await _unitIOfWork.FamilyRepository.GetAllAsync(true, IncludeExpressions.IncludeFamilyMembers);
            var familyEntity = GetFamilyEntity(CurrentUser.FamilyId, familyEntities);

            var model = new FamilyImportModel
            {
                ExistingExternalIds = familyEntities.Select(x => x.IdExternal).ToList(),
                Family = new FamilyModel
                {
                    FamilyId = familyEntity.Id,
                    IdExternal = familyEntity.IdExternal,
                    Name = familyEntity.Name,
                    ContactMailAddress = familyEntity.ContactMailAddress,
                    FamilyMembers = (from member in familyEntity.Users
                                     select new FamilyMember
                                     {
                                         IdExternal = member.IdExternal,
                                         FirstName = member.FirstName,
                                         LastName = member.LastName,
                                         DateOfBirth = member.DateOfBirth,
                                         UserRole = member.UserRole,
                                         Email = member.Email,
                                         IsActive = member.IsActive,
                                     }).ToList(),
                    IsActive = familyEntity.IsActive,
                }
            };

            var fileContent = System.Text.Json.JsonSerializer.Serialize(model, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
            });

            var file = Encoding.UTF8.GetBytes(fileContent);

            if (file == null || file.Length == 0)
            {
                return (null, null);
            }

            var familyName = string.IsNullOrEmpty(familyEntity.Name) ? "Name" : familyEntity.Name.Replace(" ", "");

            return ($"Familyimport_{familyName}_YYYYMMDD.json", Convert.ToBase64String(file));
        }

        private FamilyEntity GetFamilyEntity(int? currentUserId, List<FamilyEntity> familyEntities)
        {
            var existingFamily = currentUserId == null ?
                null :
                familyEntities.SingleOrDefault(x => x.Users.Any(x => x.Id == currentUserId));
            var currentDate = DateTime.UtcNow;

            return existingFamily != null ?
                existingFamily :
                new FamilyEntity
                {
                    Id = 0,
                    IdExternal = string.Empty,
                    Name = string.Empty,
                    ContactMailAddress = string.Empty,
                    IsActive = true,
                    Users = new List<UserEntity>
                    {
                        new UserEntity
                        {
                            Id = 0,
                            IdExternal = string.Empty,
                            FirstName = string.Empty,
                            LastName = string.Empty,
                            Username = string.Empty,
                            DateOfBirth = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0,0,0),
                            UserRole = UserRoleEnum.LocalAdmin,
                            Email = string.Empty,
                            IsActive = true,
                            Credentials = new UserCredentialsEntity
                            {
                                PasswordHash = string.Empty,
                                Salt = string.Empty,
                                RefreshToken = string.Empty,
                                ExpiresAt = DateTime.UtcNow
                            }
                        }
                    }
                };
        }


    }
}
