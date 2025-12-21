using Data.Entities;
using Data.Entities.Administation;
using Data.Shared;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Database
{
    public class AdministrationUnitOfWork : IAdministrationUnitOfWork
    {
        private ADbContext? _context;
        private IDbContextFactory _dbContextFactory;

        private IDatabaseRepositoryBase<UserEntity>? _userRepository;
        public IDatabaseRepositoryBase<UserEntity> UserRepository => 
            _userRepository ??= CreateRepository<UserEntity>();

        private IDatabaseRepositoryBase<UserRightEntity>? _userRightRepository;
        public IDatabaseRepositoryBase<UserRightEntity> UserRightRepository => 
            _userRightRepository ??= CreateRepository<UserRightEntity>();

        private IDatabaseRepositoryBase<RightEntity>? _rightRepository;
        public IDatabaseRepositoryBase<RightEntity> RightRepository => 
            _rightRepository ??= CreateRepository<RightEntity>();

        private IDatabaseRepositoryBase<LogMessageEntity>? _logRepository;
        public IDatabaseRepositoryBase<LogMessageEntity> LogRepository
            => _logRepository ??= CreateRepository<LogMessageEntity>();

        public AdministrationUnitOfWork(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            Initialize(_dbContextFactory.GetContext());
        }

        private void Initialize(ADbContext context)
        {
            _context = context;
            _userRepository = CreateRepository<UserEntity>();
            _userRightRepository = CreateRepository<UserRightEntity>();
            _rightRepository = CreateRepository<RightEntity>();
            _logRepository = CreateRepository<LogMessageEntity>();

        }

        private IDatabaseRepositoryBase<T> CreateRepository<T>() where T : AEntityBase
        {
            if (_context == null) throw new ObjectDisposedException(nameof(UnitOfWork));
            return new DatabaseRepositoryBase<T>(_context);
        }

        public async Task<int> SaveChangesAsync(
            string userName,
            CancellationToken cancellationToken = default)
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

        public async Task<int> LogMessage(
            LogMessageEntity entity,
            bool save = false,
            int? familyId = null,
            string userName = "System",
            CancellationToken cancellationToken = default)
        {
            await LogRepository.AddAsync(entity, cancellationToken);

            if (save)
            {
                return await SaveChangesAsync(userName, cancellationToken);
            }

            return 0;
        }
    }
}
