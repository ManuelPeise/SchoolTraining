using Data.Entities;
using Data.Entities.Learning;

namespace Logic.Shared.Interfaces
{
    public interface ILearningUnitOfWork : IDisposable
    {
        IDatabaseRepositoryBase<ModuleEntity> ModuleRepository { get; }
        IDatabaseRepositoryBase<SubModuleEntity> SubModuleRepository { get; }
        IDatabaseRepositoryBase<UnitEntity> UnitRepository { get; }
        IDatabaseRepositoryBase<VocabularyEntity> VocabularyRepository { get; }
        IDatabaseRepositoryBase<VocabularyUnitEntity> VocabularyUnitRepository { get; }
        IDatabaseRepositoryBase<UnitResultEntity> UnitResultRepository { get; }
        IDatabaseRepositoryBase<UserEntity> UserRepository { get; }
        IDatabaseRepositoryBase<LogMessageEntity> LogMessageRepository { get; }
        Task<int> SaveChangesAsync(string userName, CancellationToken cancellationToken = default);
        public Task<int> LogMessage(LogMessageEntity entity, bool save = false, int? familyId = null, string userName = "System", CancellationToken cancellationToken = default);
    }
}
