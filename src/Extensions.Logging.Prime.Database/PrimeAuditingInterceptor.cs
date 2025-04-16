using Extensions.Logging.Prime.Database.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Extensions.Logging.Prime.Database.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Extensions.Logging.Prime.Database
{
    public class PrimeAuditingInterceptor : ISaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContextOptions<LogDbContext> _options;
        private SaveChangesAudit _audit;

        public PrimeAuditingInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = GetDbContextOptions();
        }

        private DbContextOptions<LogDbContext> GetDbContextOptions()
        {
            var options = PrimeOptionsConfig.DbProvider switch
            {
                DatabaseType.SqlServer => new DbContextOptionsBuilder<LogDbContext>().UseSqlServer(PrimeOptionsConfig.ConnectionString).Options,
                DatabaseType.MySql => new DbContextOptionsBuilder<LogDbContext>().UseMySql(PrimeOptionsConfig.ConnectionString, ServerVersion.AutoDetect(PrimeOptionsConfig.ConnectionString)).Options,
                DatabaseType.PostgreSQL => new DbContextOptionsBuilder<LogDbContext>().UseNpgsql(PrimeOptionsConfig.ConnectionString).Options,
                _ => throw new NotSupportedException($"Database type '{PrimeOptionsConfig.DbProvider}' is not supported.")
            };
            return options;
        }

        #region SavingChanges
        public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            _audit = CreateAudit(eventData.Context);

            using var auditContext = new LogDbContext(_options);

            auditContext.Add(_audit);
            await auditContext.SaveChangesAsync();

            return result;
        }

        public InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            _audit = CreateAudit(eventData.Context);

            using var auditContext = new LogDbContext(_options);
            auditContext.Add(_audit);
            auditContext.SaveChanges();

            return result;
        }
        #endregion

        #region SavedChanges
        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            using var auditContext = new LogDbContext(_options);

            auditContext.Attach(_audit);
            _audit.Succeeded = true;
            _audit.EndTime = DateTime.UtcNow;

            auditContext.SaveChanges();

            return result;
        }

        public async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            using var auditContext = new LogDbContext(_options);

            auditContext.Attach(_audit);
            _audit.Succeeded = true;
            _audit.EndTime = DateTime.UtcNow;

            await auditContext.SaveChangesAsync(cancellationToken);

            return result;
        }
        #endregion

        #region SaveChangesFailed
        public void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            using var auditContext = new LogDbContext(_options);

            auditContext.Attach(_audit);
            _audit.Succeeded = false;
            _audit.EndTime = DateTime.UtcNow;
            _audit.ErrorMessage = eventData.Exception.Message;

            auditContext.SaveChanges();
        }

        public async Task SaveChangesFailedAsync(
            DbContextErrorEventData eventData,
            CancellationToken cancellationToken = default)
        {
            using var auditContext = new LogDbContext(_options);

            auditContext.Attach(_audit);
            _audit.Succeeded = false;
            _audit.EndTime = DateTime.UtcNow;
            _audit.ErrorMessage = eventData.Exception.InnerException?.Message;

            await auditContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region CreateAudit
        private SaveChangesAudit CreateAudit(DbContext context)
        {
            context.ChangeTracker.DetectChanges();

            var (userId, userName) = GetUserId();
            var audit = new SaveChangesAudit { UserId = userId, UserName = userName, StartTime = DateTime.UtcNow };

            foreach (var entry in context.ChangeTracker.Entries())
            {
                var auditMessage = entry.State switch
                {
                    EntityState.Deleted => CreateDeletedMessage(entry),
                    EntityState.Modified => CreateModifiedMessage(entry),
                    EntityState.Added => CreateAddedMessage(entry),
                    _ => null
                };

                if (auditMessage != null)
                {
                    audit.Entities.Add(new EntityAudit { State = entry.State, AuditMessage = auditMessage });
                }
            }

            return audit;

            string CreateAddedMessage(EntityEntry entry)
                => entry.Properties.Aggregate(
                    $"Inserting {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");

            string CreateModifiedMessage(EntityEntry entry)
                => entry.Properties.Where(property => property.IsModified || property.Metadata.IsPrimaryKey()).Aggregate(
                    $"Updating {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");

            string CreateDeletedMessage(EntityEntry entry)
                => entry.Properties.Where(property => property.Metadata.IsPrimaryKey()).Aggregate(
                    $"Deleting {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");
        }

        private (string, string) GetUserId()
        {
            var accessor = _serviceProvider.GetService<IHttpContextAccessor>();
            if (accessor?.HttpContext == null) return (string.Empty, string.Empty);
            if (accessor.HttpContext.User.Identity == null || !accessor.HttpContext.User.Identity.IsAuthenticated) return (string.Empty, string.Empty);
            var uid = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var uname = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            return (uid?.Value ?? string.Empty, uname?.Value ?? string.Empty);
        }
        #endregion
    }
}
