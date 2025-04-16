using Extensions.Logging.Prime.Database.Entity;
using Extensions.Logging.Prime.Model;

namespace Extensions.Logging.Prime.Service
{
    internal interface ILogService
    {
        Task<PrimeApiResult<StatsModel>> GetTodayStatsAsync();

        PagedResult<AppLogEntity> GetAppLogs(int index, int size);

        Task<PagedResult<AppLogEntity>> GetAppLogsAsync(int index, int size);

        Task<int> DeleteAppLogs(long[] ids);

        Task<int> DeleteAppLogsAsync(long[] ids);

        PagedResult<HttpLogEntity> GetHttpLogs(int index, int size);

        Task<PagedResult<HttpLogEntity>> GetHttpLogsAsync(int index, int size);

        Task<int> DeleteHttpLogs(long[] ids);

        Task<int> DeleteHttpLogsAsync(long[] ids);

        PagedResult<ExceptionLogEntity> GetExceptionLogs(int index, int size);

        Task<PagedResult<ExceptionLogEntity>> GetExceptionLogsAsync(int index, int size);

        Task<int> DeleteExceptionLogs(long[] ids);

        Task<int> DeleteExceptionLogsAsync(long[] ids);

        PagedResult<SaveChangesAudit> GetAuditLogs(int index, int size);

        Task<PagedResult<SaveChangesAudit>> GetAuditLogsAsync(int index, int size);

        Task<int> DeleteAuditLogs(int[] ids);

        Task<int> DeleteAuditLogsAsync(int[] ids);
    }
}
