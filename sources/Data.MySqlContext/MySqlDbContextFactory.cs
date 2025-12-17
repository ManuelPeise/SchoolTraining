using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.MySqlContext
{
    public class MySqlDbContextFactory : IDesignTimeDbContextFactory<MySqlDbContext>
    {
        public MySqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();
            optionsBuilder.UseMySql(
               "Server=localhost;Port=3306;Database=TestSchoolAppDb;User=DevUser;Password=Pass@word;",
                new MySqlServerVersion(new Version(8, 0, 21))
            );
            return new MySqlDbContext(optionsBuilder.Options);
        }
    }
}
