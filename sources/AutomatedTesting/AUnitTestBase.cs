

using Data.Testing;
using Microsoft.EntityFrameworkCore;

namespace AutomatedTesting
{
    public abstract class AUnitTestBase: IClassFixture<Startup>
    {
        private readonly TestMySqlDbContext _dbContext;
        public TestMySqlDbContext DbContext => _dbContext;

        protected AUnitTestBase(Startup startup) 
        { 
            _dbContext = startup.DbContext;
        }

        protected void EnsureMigrated()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.Migrate();
        }
    }
}
