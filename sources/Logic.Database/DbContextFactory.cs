using Data.MySqlContext;
using Data.Shared;
using Data.SqliteContext;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;

namespace Logic.Database
{
    /// <summary>
    /// Provides a factory for creating instances of database contexts based on the specified context type.
    /// </summary>
    /// <remarks>Use this class to obtain a database context instance for a given database type, such as
    /// SQLite or MySQL. The factory resolves contexts using dependency injection and ensures that each context is
    /// created within its own service scope. This approach helps manage context lifetimes and dependencies
    /// appropriately.</remarks>
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceProvider _provider;

        public DbContextFactory(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        /// <summary>
        /// Creates and returns a database context instance of the specified type. IMPORTANT: DO not use DbContextType.MsSql on .Net Maui Project.
        /// </summary>
        /// <param name="contextType">The type of database context to create. Defaults to <see cref="DbContextTypeEnum.SqLite"/> if not specified.</param>
        /// <returns>An instance of <see cref="ADbContext"/> corresponding to the specified <paramref name="contextType"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="contextType"/> is not a supported value.</exception>
        public ADbContext GetContext(DbContextTypeEnum? contextType = DbContextTypeEnum.SqLite)
        {
            // Let DI container manage the DbContext lifetime - do not create a separate scope here
            switch (contextType)
            {
                case DbContextTypeEnum.SqLite:
                    return _provider.GetRequiredService<SqLiteDbContext>();
                case DbContextTypeEnum.MySql:
                    return _provider.GetRequiredService<MySqlDbContext>();
                default: throw new ArgumentOutOfRangeException(nameof(contextType), contextType, null);
            }
        }
    }
}
