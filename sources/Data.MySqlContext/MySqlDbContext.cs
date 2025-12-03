using Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace Data.MySqlContext
{
    public class MySqlDbContext: ADbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> opt) : base(opt) { }

    }
}
