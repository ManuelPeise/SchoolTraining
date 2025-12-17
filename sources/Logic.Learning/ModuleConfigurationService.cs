using Data.Entities;
using Data.Entities.Learning;
using Logic.Learning.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Learning;

namespace Logic.Learning
{
    public class ModuleConfigurationService : LogicBase, IModuleConfigurationService
    {
        private readonly ILearningUnitOfWork _unitOfWork;

        public ModuleConfigurationService(
            ILearningUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationDataResponse<ModuleInitializationModel>> GetModuleConfigurationAsync()
        {
            try
            {
                var modules = await GetModulesAsync();
                var subModules = await GetSubModulesAsync();

                return new NotificationDataResponse<ModuleInitializationModel>
                {
                    Success = true,
                    Data = new ModuleInitializationModel
                    {
                        Modules = modules,
                        SubModules = subModules
                    }
                };
            }
            catch (Exception exception)
            {

                await LogError("A error occurred while loading module configuration initialization data from database.", exception);

                return new NotificationDataResponse<ModuleInitializationModel>
                {
                    Success = true,
                    ResourceKey = "common.errorWhileLoadingModuleInitializationModel",
                    Data = new ModuleInitializationModel
                    {
                        Modules = new List<Module>(),
                        SubModules = new List<SubModule>()
                    }
                };
            }
        }

        public async Task<NotificationDataResponse<List<Module>>> SaveOrUpdateModule(Module module)
        {
            try
            {
                var isDatabaseChanged = false;

                if (IsNewModel(module.IdExternal))
                {
                    await _unitOfWork.ModuleRepository.AddAsync(new ModuleEntity
                    {
                        IdExternal = Guid.NewGuid().ToString(),
                        Title = module.Title,
                        Description = module.Description,
                    });

                    await LogInfo($"New module '{module.Title}' is created by user '{CurrentUser.UserName}'.");

                    isDatabaseChanged = true;
                }
                else
                {
                    var existingEntity = await _unitOfWork.ModuleRepository.GetByAsync(e => e.IdExternal == module.IdExternal, true);

                    if (existingEntity != null)
                    {
                        existingEntity.Title = module.Title;
                        existingEntity.Description = module.Description;
                        existingEntity.UpdatedAt = DateTime.UtcNow;
                        existingEntity.UpdatedBy = CurrentUser.UserName;

                        _unitOfWork.ModuleRepository.Update(existingEntity);

                        await LogInfo($"Module '{module.Title}' is updated by user '{CurrentUser.UserName}'.");

                        isDatabaseChanged = true;
                    }
                }

                if (isDatabaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return new NotificationDataResponse<List<Module>>
                {
                    Success = true,
                    Data = await GetModulesAsync()
                };
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while saving or updating a module.", exception);

                return new NotificationDataResponse<List<Module>>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileSavingOrUpdatingModule",
                    Data = await GetModulesAsync()
                };
            }
        }

        public async Task<NotificationDataResponse<List<SubModule>>> SaveOrUpdateSubModule(SubModule subModule)
        {
            try
            {
                var isDatabaseChanged = false;

                if (IsNewModel(subModule.IdExternal))
                {
                    await _unitOfWork.SubModuleRepository.AddAsync(new SubModuleEntity
                    {
                        IdExternal = Guid.NewGuid().ToString(),
                        ModuleId = subModule.ModuleId,
                        Title = subModule.Title,
                        Description = subModule.Description,
                        VocabularyDirection = subModule.Direction,
                        Units = new List<UnitEntity>()
                    });

                    await LogInfo($"New sub module '{subModule.Title}' is created by user '{CurrentUser.UserName}'.");

                    isDatabaseChanged = true;
                }
                else
                {
                    var existingEntity = await _unitOfWork.SubModuleRepository.GetByAsync(e => e.IdExternal == subModule.IdExternal);

                    if (existingEntity != null)
                    {
                        existingEntity.Title = subModule.Title;
                        existingEntity.Description = subModule.Description;
                        existingEntity.VocabularyDirection = subModule.Direction;
                        existingEntity.Units = existingEntity.Units ?? new List<UnitEntity>();

                        await LogInfo($"Sub module '{subModule.Title}' is updated by user '{CurrentUser.UserName}'.");

                        isDatabaseChanged = true;
                    }
                }

                if (isDatabaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return new NotificationDataResponse<List<SubModule>>
                {
                    Success = true,
                    Data = await GetSubModulesAsync()
                };
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while saving or updating a sub module.", exception);

                return new NotificationDataResponse<List<SubModule>>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileSavingOrUpdatingSubModule",
                    Data = await GetSubModulesAsync()
                };
            }
        }

        public async Task<NotificationDataResponse<List<Module>>> DeleteModule(string idExternal)
        {
            try
            {
                var existingEntity = await _unitOfWork.ModuleRepository.GetByAsync(e => e.IdExternal == idExternal);

                if (existingEntity != null)
                {
                    _unitOfWork.ModuleRepository.Remove(existingEntity);

                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);

                    await LogInfo($"Module '{existingEntity.Title}' is deleted by user '{CurrentUser.UserName}'.", true);

                    return new NotificationDataResponse<List<Module>>
                    {
                        Success = true,
                        Data = await GetModulesAsync()
                    };
                }

                await LogError("A error occurred while deleting a module.", null);

                return new NotificationDataResponse<List<Module>>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileDeletingModule",
                    Data = await GetModulesAsync()
                };
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while deleting a module.", exception);

                return new NotificationDataResponse<List<Module>>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileDeletingModule",
                    Data = await GetModulesAsync()
                };
            }
        }

        private async Task<List<Module>> GetModulesAsync()
        {
            var entities = await _unitOfWork.ModuleRepository.GetAllAsync(true);

            return entities?.Select(e => new Module
            {
                ModuleId = e.Id,
                IdExternal = e.IdExternal,
                Title = e.Title,
                Description = e.Description,
                LastUpdateBy = GetLastUpdateBy(e.UpdatedBy, e.CreatedBy),
                LastUpdateAt = GetLastUpdateAt(e.UpdatedAt, e.CreatedAt)
            }).ToList() ?? new List<Module>();
        }

        private async Task<List<SubModule>> GetSubModulesAsync()
        {
            var entities = await _unitOfWork.SubModuleRepository.GetAllAsync(true);

            return entities?.Select(e => new SubModule
            {
                SubModuleId = e.Id,
                IdExternal = e.IdExternal,
                Title = e.Title,
                Description = e.Description,
                Direction = e.VocabularyDirection,
                LastUpdateBy = GetLastUpdateBy(e.UpdatedBy, e.CreatedBy),
                LastUpdateAt = GetLastUpdateAt(e.UpdatedAt, e.CreatedAt)
            }).ToList() ?? new List<SubModule>();
        }

        private bool IsNewModel(string idExternal)
        {
            return string.IsNullOrEmpty(idExternal);
        }

        private DateTime GetLastUpdateAt(DateTime? updetedAt, DateTime createdAt)
        {
            return updetedAt == null || updetedAt == DateTime.MinValue ? createdAt : updetedAt.Value;
        }

        private string GetLastUpdateBy(string? updetedAt, string createdAt)
        {
            return string.IsNullOrEmpty(updetedAt) ? createdAt : updetedAt;
        }

        private async Task LogInfo(string message, bool save = false)
        {
            await _unitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                Module = nameof(ModuleConfigurationService),
                LogLevel = LogLevelEnum.Error,
                TimeStamp = DateTime.UtcNow
            }, save, CurrentUser.FamilyId, CurrentUser.UserName);
        }

        private async Task LogError(string message, Exception? exception, bool save = true)
        {
            await _unitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = exception?.Message ?? string.Empty,
                StackTrace = exception?.StackTrace,
                Module = nameof(ModuleConfigurationService),
                LogLevel = LogLevelEnum.Error,
                TimeStamp = DateTime.UtcNow
            }, save, CurrentUser.FamilyId, CurrentUser.UserName);
        }
    }
}
