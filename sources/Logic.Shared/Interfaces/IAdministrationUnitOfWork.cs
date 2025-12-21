using Data.Entities;
using Data.Entities.Administation;

namespace Logic.Shared.Interfaces
{
    public interface IAdministrationUnitOfWork
    {
        public IDatabaseRepositoryBase<UserEntity> UserRepository { get; }
        public IDatabaseRepositoryBase<UserRightEntity> UserRightRepository { get; }
        public IDatabaseRepositoryBase<RightEntity> RightRepository { get; }
        public IDatabaseRepositoryBase<LogMessageEntity> LogRepository { get; }
        Task<int> SaveChangesAsync(
            string userName,
            CancellationToken cancellationToken = default);
        Task<int> LogMessage(
            LogMessageEntity entity,
            bool save = false,
            int? familyId = null,
            string userName = "System",
            CancellationToken cancellationToken = default);
    }
}
