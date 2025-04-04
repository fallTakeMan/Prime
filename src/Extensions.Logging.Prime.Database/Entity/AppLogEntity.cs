using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Extensions.Logging.Prime.Database.Entity
{
    [Table("AppLogs")]
    public class AppLogEntity : BaseEntity
    {
        [Comment("Event Id")]
        public int EventId { get; set; }

        [Comment("Event Name")]
        [Column(TypeName = "VARCHAR(200)")]
        public string? EventName { get; set; }

        [Comment("日志级别")]
        [Column(TypeName = "VARCHAR(50)")]
        public required string LogLevel { get; set; }

        [Comment("Logger")]
        [Column(TypeName = "VARCHAR(500)")]
        public required string Logger { get; set; }

        [Comment("日志内容")]
        [Column(TypeName = "TEXT")]
        public required string Message { get; set; }

    }
}
