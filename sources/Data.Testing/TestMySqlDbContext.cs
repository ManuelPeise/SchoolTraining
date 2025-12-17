using Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace Data.Testing
{
    public class TestMySqlDbContext : ADbContext
    {
        public TestMySqlDbContext(DbContextOptions<TestMySqlDbContext> options) : base(options) { }

    }
}
