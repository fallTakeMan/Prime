using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Extensions.Logging.Prime.Database.Entity
{
    [Table("ExceptionLogs")]
    internal class ExceptionLogEntity : BaseEntity
    {
        [Comment("请求方法")]
        [Column(TypeName = "VARCHAR(10)")]
        public string? Method { get; set; }

        [Comment("服务主机")]
        [Column(TypeName = "VARCHAR(200)")]
        public string? Host { get; set; }

        [Comment("请求路径")]
        [Column(TypeName = "TEXT")]
        public string? Path { get; set; }

        [Comment("请求url参数")]
        [Column(TypeName = "TEXT")]
        public string? QueryString { get; set; }

        [Comment("请求头")]
        [Column(TypeName = "TEXT")]
        public string? RequestHeaders { get; set; }

        [Comment("请求消息体")]
        [Column(TypeName = "TEXT")]
        public string? RequestBody { get; set; }

        [Comment("客户端ip")]
        [Column(TypeName = "VARCHAR(64)")]
        public string? IpAddress { get; set; }

        [Comment("请求连接id")]
        [Column(TypeName = "VARCHAR(64)")]
        public string? ConnectionId { get; set; }

        [Comment("追踪id")]
        [Column(TypeName = "VARCHAR(64)")]
        public string? TraceId { get; set; }

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

        [Comment("异常源")]
        [Column(TypeName = "TEXT")]
        public string? ExceptionSource { get; set; }

        [Comment("异常消息")]
        [Column(TypeName = "TEXT")]
        public string? ExceptionMessage { get; set; }

        [Comment("异常堆栈")]
        [Column(TypeName = "TEXT")]
        public string? ExceptionStackTrace { get; set; }
    }
}
