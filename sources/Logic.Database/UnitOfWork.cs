using Data.Entities;
using Data.Shared;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Logic.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly DbContextTypeEnum? _dbContextType;
        private ADbContext? _context;

        private IDatabaseRepositoryBase<FamilyEntity>? _familyRepository;
        public IDatabaseRepositoryBase<FamilyEntity> FamilyRepository =>
            _familyRepository ??= CreateRepository<FamilyEntity>();


        private IDatabaseRepositoryBase<UserEntity>? _userRepository;
        public IDatabaseRepositoryBase<UserEntity> UserRepository =>
            _userRepository ??= CreateRepository<UserEntity>();


        private IDatabaseRepositoryBase<UserCredentialsEntity>? _userCredentialsRepository;
        public IDatabaseRepositoryBase<UserCredentialsEntity> UserCredentialsRepository =>
            _userCredentialsRepository ??= CreateRepository<UserCredentialsEntity>();


        private IDatabaseRepositoryBase<UserSettingsEntity>? _userSettingsRepository;
        public IDatabaseRepositoryBase<UserSettingsEntity> UserSettingsRepository =>
            _userSettingsRepository ??= CreateRepository<UserSettingsEntity>();


        private IDatabaseRepositoryBase<LogMessageEntity>? _logMessageRepository;
        public IDatabaseRepositoryBase<LogMessageEntity> LogMessageRepository =>
            _logMessageRepository ??= CreateRepository<LogMessageEntity>();

        private IDatabaseRepositoryBase<ImportFileEntity>? _importFileRepository;
        public IDatabaseRepositoryBase<ImportFileEntity> ImportFileRepository =>
            _importFileRepository ??= CreateRepository<ImportFileEntity>();

        public UnitOfWork(IDbContextFactory dbContextFactory, DbContextTypeEnum? dbContextTypeEnum)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _dbContextType = dbContextTypeEnum;
            _context = dbContextFactory.GetContext(dbContextTypeEnum);
        }

        private IDatabaseRepositoryBase<T> CreateRepository<T>() where T : AEntityBase
        {
            if (_context == null) throw new ObjectDisposedException(nameof(UnitOfWork));
            return new DatabaseRepositoryBase<T>(_context);
        }

        public async Task<int> SaveChangesAsync(string userName, CancellationToken cancellationToken = default)
        {
            if (_context == null) throw new ObjectDisposedException(nameof(UnitOfWork));

            var now = DateTime.UtcNow;

            var entries = _context.ChangeTracker.Entries<AEntityBase>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.CreatedBy = userName ?? string.Empty;
                    entry.Entity.UpdatedBy = userName ?? string.Empty;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(AEntityBase.CreatedAt)).IsModified = false;
                    entry.Property(nameof(AEntityBase.CreatedBy)).IsModified = false;

                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userName ?? string.Empty;
                }
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> LogMessage(LogMessageEntity entity, bool save = false, string userName = "System", CancellationToken cancellationToken = default)
        {
            await LogMessageRepository.AddAsync(entity, cancellationToken);

            if (save)
            {
                return await SaveChangesAsync(userName, cancellationToken);
            }

            return 0;
        }
        #region dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
