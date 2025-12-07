using Data.Entities;
using Data.MySqlContext;
using Logic.Shared.Interfaces;

namespace Logic.Shared
{
    public class LogService: ILogService
    {
        private readonly MySqlDbContext _context;

        public LogService(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task LogMessage(LogMessageEntity entity)
        {
            await _context.LogMessageTable.AddAsync(entity);

            await _context.SaveChangesAsync();
        }
    }
}
