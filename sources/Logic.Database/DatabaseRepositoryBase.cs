using Data.Entities;
using Data.Shared;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Logic.Database
{
    public class DatabaseRepositoryBase<TEntity> : IDatabaseRepositoryBase<TEntity> where TEntity : AEntityBase
    {
        private ADbContext _context { get; }

        public DatabaseRepositoryBase(ADbContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetAllAsync(bool asNoTracking = true, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(int id, bool asNoTracking = true, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> expression, bool asNoTracking = true, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(expression);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async Task AddIfNotExistAsync(TEntity entity, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            var exists = await _context.Set<TEntity>().AnyAsync(expression, cancellationToken);
            if (!exists)
            {
                await AddAsync(entity, cancellationToken);
            }
        }

        public async Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity.Id == 0)
            {
                await AddAsync(entity, cancellationToken);
                return;
            }

            var exists = await _context.Set<TEntity>().AnyAsync(e => e.Id == entity.Id, cancellationToken);
            if (exists)
            {
                Update(entity);
            }
            else
            {
                await AddAsync(entity, cancellationToken);
            }
        }

        public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
