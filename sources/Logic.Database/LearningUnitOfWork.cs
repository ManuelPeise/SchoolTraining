using Data.Entities;
using Data.Entities.Learning;
using Data.Shared;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Database
{
    public class LearningUnitOfWork : ILearningUnitOfWork
    {
        private ADbContext? _context;
        private IDbContextFactory _dbContextFactory;

        private IDatabaseRepositoryBase<ModuleEntity>? _moduleRepository;
        private IDatabaseRepositoryBase<SubModuleEntity>? _subModuleRepository;
        private IDatabaseRepositoryBase<UnitEntity>? _unitRepository;
        private IDatabaseRepositoryBase<VocabularyEntity>? _vocabularyRepository;
        private IDatabaseRepositoryBase<VocabularyUnitEntity>? _vocabularyUnitRepository;
        private IDatabaseRepositoryBase<UnitResultEntity>? _unitResultRepository;
        private IDatabaseRepositoryBase<UserEntity>? _userRepository;
        private IDatabaseRepositoryBase<LogMessageEntity>? _logMessageRepository;

        public LearningUnitOfWork(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            Initialize(_dbContextFactory.GetContext()); ;
        }

        private void Initialize(ADbContext context)
        {
            _context = context;
            _logMessageRepository = CreateRepository<LogMessageEntity>();
            _moduleRepository = CreateRepository<ModuleEntity>();
            _subModuleRepository = CreateRepository<SubModuleEntity>();
            _unitRepository = CreateRepository<UnitEntity>();
            _unitResultRepository = CreateRepository<UnitResultEntity>();
            _userRepository = CreateRepository<UserEntity>();
            _vocabularyRepository = CreateRepository<VocabularyEntity>();
            _vocabularyUnitRepository = CreateRepository<VocabularyUnitEntity>();
        }

        public IDatabaseRepositoryBase<ModuleEntity> ModuleRepository =>
            _moduleRepository ??= CreateRepository<ModuleEntity>();
        public IDatabaseRepositoryBase<SubModuleEntity> SubModuleRepository =>
            _subModuleRepository ??= CreateRepository<SubModuleEntity>();
        public IDatabaseRepositoryBase<UnitEntity> UnitRepository =>
            _unitRepository ??= CreateRepository<UnitEntity>();
        public IDatabaseRepositoryBase<VocabularyEntity> VocabularyRepository =>
            _vocabularyRepository ??= CreateRepository<VocabularyEntity>();
        public IDatabaseRepositoryBase<VocabularyUnitEntity> VocabularyUnitRepository =>
            _vocabularyUnitRepository ??= CreateRepository<VocabularyUnitEntity>();
        public IDatabaseRepositoryBase<UnitResultEntity> UnitResultRepository =>
            _unitResultRepository ??= CreateRepository<UnitResultEntity>();
        public IDatabaseRepositoryBase<UserEntity> UserRepository =>
            _userRepository ??= CreateRepository<UserEntity>();
        public IDatabaseRepositoryBase<LogMessageEntity> LogMessageRepository =>
            _logMessageRepository ??= CreateRepository<LogMessageEntity>();

        private IDatabaseRepositoryBase<T> CreateRepository<T>() where T : AEntityBase
        {
            if (_context == null) throw new ObjectDisposedException(nameof(UnitOfWork));
            return new DatabaseRepositoryBase<T>(_context);
        }

        public async  Task<int> SaveChangesAsync(
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
