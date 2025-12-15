using Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace Data.SqliteContext
{
    public class SqLiteDbContext : ADbContext
    {
        public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options) { }

    }
}
