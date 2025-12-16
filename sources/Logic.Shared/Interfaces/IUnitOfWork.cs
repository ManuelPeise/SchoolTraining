using Data.Entities;

namespace Logic.Shared.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IDatabaseRepositoryBase<UserEntity> UserRepository { get; }
        IDatabaseRepositoryBase<UserCredentialsEntity> UserCredentialsRepository { get; }
        IDatabaseRepositoryBase<UserSettingsEntity> UserSettingsRepository { get; }
        IDatabaseRepositoryBase<FamilyEntity> FamilyRepository { get; }
        IDatabaseRepositoryBase<ImportFileEntity> ImportFileRepository { get; }
        IDatabaseRepositoryBase<LogMessageEntity> LogMessageRepository { get; }
        ILearningUnitOfWork LearningUnitOfWork { get; }
        Task<int> SaveChangesAsync(string userName, CancellationToken cancellationToken = default);
        public Task<int> LogMessage(LogMessageEntity entity, bool save = false, int? familyId = null, string userName = "System", CancellationToken cancellationToken = default);
    }
}
