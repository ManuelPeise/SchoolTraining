using Data.Entities;
using System.Linq.Expressions;

namespace Logic.Shared.Interfaces
{
    public interface IDatabaseRepositoryBase<TEntity> where TEntity : AEntityBase
    {
        /// <summary>
        /// Return all entities.
        /// </summary>
        Task<List<TEntity>> GetAllAsync(
            bool asNoTracking = true, 
            IEnumerable<Expression<Func<TEntity, object>>>? includes = null, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Return an entity by its primary key or null if not found.
        /// </summary>
        Task<TEntity?> GetByIdAsync(
            int id, 
            bool asNoTracking = true, 
            IEnumerable<Expression<Func<TEntity, object>>>? includes = null, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Return an entity by a predicate or null if not found.
        /// </summary>
        Task<TEntity?> GetByAsync(
            Expression<Func<TEntity, bool>> expression, 
            bool asNoTracking = true, 
            IEnumerable<Expression<Func<TEntity, object>>>? includes = null, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a new entity.
        /// </summary>
        Task AddAsync(
            TEntity entity, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a new entity if no entity matches the given predicate.
        /// </summary>
        Task AddIfNotExistAsync(
            TEntity entity, 
            Expression<Func<TEntity, bool>> expression, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add or update the entity in the current context (does not persist changes).
        /// If the entity has an identity value and exists, it will be updated; otherwise it will be added.
        /// </summary>
        Task AddOrUpdateAsync(
            TEntity entity, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a range of entities.
        /// </summary>
        Task AddRangeAsync(
            List<TEntity> entities, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing entity. Changes are applied when SaveChangesAsync is called.
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Update a range of entities. Changes are applied when SaveChangesAsync is called.
        /// </summary>
        void UpdateRange(List<TEntity> entities);

        /// <summary>
        /// Remove an entity. Changes are applied when SaveChangesAsync is called.
        /// </summary>
        void Remove(TEntity entity);

    }
}
