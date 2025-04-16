
namespace Extensions.Logging.Prime.Model
{
    internal class StatsModel
    {
        public int AppLogRecords { get; set; }
        public int AppLogsToday { get; set; }

        public int HttpLogRecords { get; set; }

        public int HttpLogsToday { get; set; }

        public int ExceptionLogRecords { get; set; }

        public int ExceptionLogsToday { get; set; }

        public int AuditLogRecords { get; set; }

        public int AuditLogsToday { get; set; }
    }
}
