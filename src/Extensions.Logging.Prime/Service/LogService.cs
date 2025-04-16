using Extensions.Logging.Prime.Database;
using Extensions.Logging.Prime.Database.Entity;
using Extensions.Logging.Prime.Model;
using Microsoft.EntityFrameworkCore;

namespace Extensions.Logging.Prime.Service
{
    internal class LogService : ILogService
    {
        private readonly LogDbContext _dbContext;

        public LogService(LogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PrimeApiResult<StatsModel>> GetTodayStatsAsync()
        {
            int appLogRecords = await _dbContext.AppLogs.CountAsync();
            int appLogsToday = await _dbContext.AppLogs.Where(x => x.LogTime > DateTime.UtcNow.Date).CountAsync();
            int httpLogRecords = await _dbContext.HttpLogs.CountAsync();
            int httpLogsToday = await _dbContext.HttpLogs.Where(x => x.LogTime > DateTime.UtcNow.Date).CountAsync();
            int exceptionLogRecords = await _dbContext.ExceptionLogs.CountAsync();
            int exceptionLogsToday = await _dbContext.ExceptionLogs.Where(x => x.LogTime > DateTime.UtcNow.Date).CountAsync();
            int auditLogRecords = await _dbContext.SaveChangesAudits.CountAsync();
            int auditLogsToday = await _dbContext.SaveChangesAudits.Where(x => x.StartTime > DateTime.UtcNow.Date).CountAsync();
            var data = new StatsModel
            {
                AppLogRecords = appLogRecords,
                AppLogsToday = appLogsToday,
                HttpLogRecords = httpLogRecords,
                HttpLogsToday = httpLogsToday,
                ExceptionLogRecords = exceptionLogRecords,
                ExceptionLogsToday = exceptionLogsToday,
                AuditLogRecords = auditLogRecords,
                AuditLogsToday = auditLogsToday
            };
            return PrimeApiResult<StatsModel>.Success(data);
        }

        public PagedResult<AppLogEntity> GetAppLogs(int index, int size)
        {
            var query = from log in _dbContext.AppLogs
                        orderby log.LogTime descending
                        select log;
            var total = query.Count();
            var list = query.Skip((index - 1) * size).Take(size).AsNoTracking().ToList();
            return new PagedResult<AppLogEntity>(list, index, size, total);
        }

        public async Task<PagedResult<AppLogEntity>> GetAppLogsAsync(int index, int size)
        {
            var query = from log in _dbContext.AppLogs
                        orderby log.LogTime descending
                        select log;
            var total = await query.CountAsync();
            var list = await query.Skip((index - 1) * size).Take(size).AsNoTracking().ToListAsync();
            return new PagedResult<AppLogEntity>(list, index, size, total);
        }

        public Task<int> DeleteAppLogs(long[] ids)
        {
            var result = _dbContext.AppLogs.Where(x => ids.Contains(x.Id)).ExecuteDelete();
            return Task.FromResult(result);
        }

        public async Task<int> DeleteAppLogsAsync(long[] ids)
        {
            return await _dbContext.AppLogs.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
        }

        public PagedResult<HttpLogEntity> GetHttpLogs(int index, int size)
        {
            var query = from log in _dbContext.HttpLogs
                        orderby log.LogTime descending
                        select log;
            var total = query.Count();
            var list = query.Skip((index - 1) * size).Take(size).AsNoTracking().ToList();
            return new PagedResult<HttpLogEntity>(query, index, size, total);
        }

        public async Task<PagedResult<HttpLogEntity>> GetHttpLogsAsync(int index, int size)
        {
            var query = from log in _dbContext.HttpLogs
                        orderby log.LogTime descending
                        select log;
            var total = await query.CountAsync();
            var list = await query.Skip((index - 1) * size).Take(size).AsNoTracking().ToListAsync();
            return new PagedResult<HttpLogEntity>(list, index, size, total);
        }

        public Task<int> DeleteHttpLogs(long[] ids)
        {
            var result = _dbContext.HttpLogs.Where(x => ids.Contains(x.Id)).ExecuteDelete();
            return Task.FromResult(result);
        }

        public async Task<int> DeleteHttpLogsAsync(long[] ids)
        {
            return await _dbContext.HttpLogs.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
        }

        public PagedResult<ExceptionLogEntity> GetExceptionLogs(int index, int size)
        {
            var query = from log in _dbContext.ExceptionLogs
                        orderby log.LogTime descending
                        select log;
            var total = query.Count();
            var list = query.Skip((index - 1) * size).Take(size).AsNoTracking().ToList();
            return new PagedResult<ExceptionLogEntity>(list, index, size, total);
        }

        public async Task<PagedResult<ExceptionLogEntity>> GetExceptionLogsAsync(int index, int size)
        {
            var query = from log in _dbContext.ExceptionLogs
                        orderby log.LogTime descending
                        select log;
            var total = await query.CountAsync();
            var list = await query.Skip((index - 1) * size).Take(size).AsNoTracking().ToListAsync();
            return new PagedResult<ExceptionLogEntity>(list, index, size, total);
        }

        public Task<int> DeleteExceptionLogs(long[] ids)
        {
            var result = _dbContext.ExceptionLogs.Where(x => ids.Contains(x.Id)).ExecuteDelete();
            return Task.FromResult(result);
        }

        public async Task<int> DeleteExceptionLogsAsync(long[] ids)
        {
            return await _dbContext.ExceptionLogs.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
        }

        #region Audit logs
        public PagedResult<SaveChangesAudit> GetAuditLogs(int index, int size)
        {
            var query = from log in _dbContext.SaveChangesAudits.Include(x => x.Entities)
                        orderby log.StartTime descending
                        select log;
            var total = query.Count();
            var list = query.Skip((index - 1) * size).Take(size).AsNoTracking().ToList();
            return new PagedResult<SaveChangesAudit>(list, index, size, total);
        }

        public async Task<PagedResult<SaveChangesAudit>> GetAuditLogsAsync(int index, int size)
        {
            var query = from log in _dbContext.SaveChangesAudits.Include(x => x.Entities)
                        orderby log.StartTime descending
                        select log;
            var total = await query.CountAsync();
            var list = await query.Skip((index - 1) * size).Take(size).AsNoTracking().ToListAsync();
            return new PagedResult<SaveChangesAudit>(list, index, size, total);
        }

        public Task<int> DeleteAuditLogs(int[] ids)
        {
            var result = _dbContext.SaveChangesAudits.Include(x => x.Entities).Where(x => ids.Contains(x.Id)).ExecuteDelete();
            return Task.FromResult(result);
        }

        public async Task<int> DeleteAuditLogsAsync(int[] ids)
        {
            return await _dbContext.SaveChangesAudits.Include(x => x.Entities).Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
        } 
        #endregion
    }
}
