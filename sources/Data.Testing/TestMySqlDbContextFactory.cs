using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Data.Testing
{
    public class TestMySqlDbContextFactory : IDesignTimeDbContextFactory<TestMySqlDbContext>
    {
        public TestMySqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestMySqlDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;database=testdb;user=root;password=yourpassword;",
                new MySqlServerVersion(new Version(8, 0, 21))
            );
            return new TestMySqlDbContext(optionsBuilder.Options);
        }
    }
}
