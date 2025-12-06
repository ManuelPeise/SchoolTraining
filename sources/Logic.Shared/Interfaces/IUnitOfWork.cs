using Data.Entities;

namespace Logic.Shared.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IDatabaseRepositoryBase<UserEntity> UserRepository { get; }
        IDatabaseRepositoryBase<UserCredentialsEntity> UserCredentialsRepository { get; }
        IDatabaseRepositoryBase<UserSettingsEntity> UserSettingsRepository { get; }
        IDatabaseRepositoryBase<FamilyEntity> FamilyRepository { get; }
        IDatabaseRepositoryBase<LogMessageEntity> LogMessageRepository { get; }

        /// <summary>
        /// Save all changes made in this unit of work to the database.
        /// </summary>
        Task<int> SaveChangesAsync(string userName, CancellationToken cancellationToken = default);
        public Task<int> LogMessage(LogMessageEntity entity, bool save = false, string userName = "System", CancellationToken cancellationToken = default);
    }
}
