using Extensions.Logging.Prime.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Extensions.Logging.Prime.Database
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        public DbSet<AppLogEntity> AppLogs { get; set; }

        public DbSet<HttpLogEntity> HttpLogs { get; set; }

        public DbSet<ExceptionLogEntity> ExceptionLogs { get; set; }

        public DbSet<SaveChangesAudit> SaveChangesAudits { get; set; }
    }
}
