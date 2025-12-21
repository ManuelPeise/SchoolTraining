using Data.Entities;
using Data.Entities.Learning;
using Logic.Learning;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Models.Authentication;
using Shared.Models.Learning;
using System.Diagnostics;

namespace AutomatedTesting.Learning
{
    public class LearningUnitOfWorkTests : AUnitTestBase
    {
        private readonly LearningUnitOfWork _unitOfWork;
        private readonly string TestModuleExternalId = new Guid("dd623fcc-270c-42f4-a4e4-c31210443c3d").ToString();
        private readonly string TestSubModuleExternalId = new Guid("aab4120a-a811-4740-b7aa-8c5a5ee923a3").ToString();

        public LearningUnitOfWorkTests(Startup startup) : base(startup)
        {
            _unitOfWork = new LearningUnitOfWork(startup);
        }

        [Fact]
        public async Task ExecuteModuleTest()
        {
            try
            {
                EnsureMigrated();
                await AddModuleAsync();
                await AddSubModuleAsync();
                await GetModulesAsync();
                await GetSubModulesAsync();
                await UpdateModule();
                await UpdateSubModule();
                await DeleteModuleAsync();

            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        private async Task GetModulesAsync()
        {
            var entities = await _unitOfWork.ModuleRepository.GetAllAsync(true);

            var modules = entities?.Select(e => new Module
            {
                ModuleId = e.Id,
                IdExternal = e.IdExternal,
                Title = e.Title,
                Description = e.Description,
                LastUpdateBy = GetLastUpdateBy(e.UpdatedBy, e.CreatedBy),
                LastUpdateAt = GetLastUpdateAt(e.UpdatedAt, e.CreatedAt)
            }).ToList() ?? new List<Module>();

            Assert.True(modules != null && modules.Any());
        }

        private async Task GetSubModulesAsync()
        {
            var entities = await _unitOfWork.SubModuleRepository.GetAllAsync(true);

            var subModules = entities?.Select(e => new SubModule
            {
                ModuleId = e.Id,
                IdExternal = e.IdExternal,
                Title = e.Title,
                Description = e.Description,
                Direction = e.VocabularyDirection,
                LastUpdateBy = GetLastUpdateBy(e.UpdatedBy, e.CreatedBy),
                LastUpdateAt = GetLastUpdateAt(e.UpdatedAt, e.CreatedAt)
            }).ToList() ?? new List<SubModule>();

            Assert.True(subModules != null && subModules.Any());
        }

        private async Task AddModuleAsync()
        {
            try
            {
                var moduleEntity = new ModuleEntity
                {
                    IdExternal = TestModuleExternalId,
                    Title = "Test-Module",
                    Description = "Test-Module-Description",
                };

                await _unitOfWork.ModuleRepository.AddAsync(moduleEntity);

                await _unitOfWork.SaveChangesAsync("TestUser");

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private async Task AddSubModuleAsync()
        {
            try
            {
                var parent = await _unitOfWork.ModuleRepository.GetByAsync(x => x.IdExternal == TestModuleExternalId);

                if (parent == null)
                {
                    Assert.True(false);

                    return;
                }

                var moduleEntity = new SubModuleEntity
                {
                    ModuleId = parent.Id,
                    IdExternal = TestSubModuleExternalId,
                    Title = "Test-Sub-Module",
                    Description = "Test-Sub-Module-Description",
                    VocabularyDirection = VocabularyDirectionEnum.GermanEnglish,
                    Units = new List<UnitEntity>()
                };

                await _unitOfWork.SubModuleRepository.AddAsync(moduleEntity);

                await _unitOfWork.SaveChangesAsync("TestUser");

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private async Task UpdateModule()
        {
            try
            {
                var module = await _unitOfWork.ModuleRepository.GetByAsync(x => x.Id == 1);

                if (module == null)
                {
                    Assert.True(false);

                    return;
                }

                module.Title = "Updated-Test-Module-1";
                // _unitOfWork.ModuleRepository.Update(module);
                await _unitOfWork.SaveChangesAsync("TestUser");

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private async Task UpdateSubModule()
        {
            try
            {
                var subModule = await _unitOfWork.SubModuleRepository.GetByAsync(x => x.Id == 1);

                if (subModule == null)
                {
                    Assert.True(false);

                    return;
                }

                subModule.Title = "Updated-Test-Sub-Module-1";
                //_unitOfWork.SubModuleRepository.Update(subModule);
                await _unitOfWork.SaveChangesAsync("TestUser");

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private async Task DeleteModuleAsync()
        {
            try
            {
                var module = await _unitOfWork.ModuleRepository.GetByAsync(x => x.IdExternal == TestModuleExternalId);
                if (module == null)
                {
                    Assert.True(false);

                    return;
                }

                DbContext.ModuleTable.Remove(module);
                await DbContext.SaveChangesAsync();

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private string GetLastUpdateAt(DateTime? updetedAt, DateTime createdAt)
        {
            return updetedAt == null || updetedAt == DateTime.MinValue ? createdAt.ToString("dd.MM.yyyy HH:mm") : updetedAt.Value.ToString("dd.MM.yyyy HH:mm");
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
            }, save, null, "Test-User");
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
            }, save, null, "Test-User");
        }
    }
}
