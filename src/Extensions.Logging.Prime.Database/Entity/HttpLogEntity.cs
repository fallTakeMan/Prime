using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Extensions.Logging.Prime.Database.Entity
{
    [Table("HttpLogs")]
    public class HttpLogEntity : BaseEntity
    {
        [Comment("请求方法")]
        [Column(TypeName = "VARCHAR(10)")]
        public required string Method { get; set; }

        [Comment("服务主机")]
        [Column(TypeName = "VARCHAR(200)")]
        public required string Host { get; set; }

        [Comment("请求路径")]
        [Column(TypeName = "TEXT")]
        public required string Path { get; set; }

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

        [Comment("请求响应持续时间")]
        [Column(TypeName = "TIME")]
        public TimeSpan Duration { get; set; }

        [Comment("响应状态码")]
        public int StatusCode { get; set; }

        [Comment("响应头")]
        [Column(TypeName = "TEXT")]
        public string? ResponseHeaders { get; set; }

        [Comment("响应消息体")]
        [Column(TypeName = "TEXT")]
        public string? ResponseBody { get; set; }

    }
}
