using Extensions.Logging.Prime.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Extensions.Logging.Prime.Database
{
    internal class LogDbContext : DbContext
    {

        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<AppLogEntity> AppLogs { get; set; }

        public DbSet<HttpLogEntity> HttpLogs { get; set; }

        public DbSet<ExceptionLogEntity> ExceptionLogs { get; set; }
    }
}
