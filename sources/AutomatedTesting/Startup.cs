using Data.Testing;
using Microsoft.EntityFrameworkCore;


namespace AutomatedTesting
{
    public class Startup : IDisposable
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database='testschoolappdb';User=DevUser;Password=Pass@word;";
        public TestMySqlDbContext DbContext { get; }

        public Startup()
        {
            var options = new DbContextOptionsBuilder<TestMySqlDbContext>()
            .UseMySql(ConnectionString, new MySqlServerVersion(new Version(8, 0, 21)))
            .Options;

            DbContext = new TestMySqlDbContext(options);

           
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }

 
}
