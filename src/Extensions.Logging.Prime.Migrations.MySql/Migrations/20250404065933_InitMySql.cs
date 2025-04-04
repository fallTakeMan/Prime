using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Extensions.Logging.Prime.Migrations.MySql.Migrations
{
    /// <inheritdoc />
    public partial class InitMySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键，雪花id"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "Event Id"),
                    EventName = table.Column<string>(type: "VARCHAR(200)", nullable: true, comment: "Event Name"),
                    LogLevel = table.Column<string>(type: "VARCHAR(50)", nullable: false, comment: "日志级别"),
                    Logger = table.Column<string>(type: "VARCHAR(500)", nullable: false, comment: "Logger"),
                    Message = table.Column<string>(type: "TEXT", nullable: false, comment: "日志内容"),
                    LogTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "UTC时间"),
                    UserId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "系统登录用户id"),
                    UserName = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "系统登录用户名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键，雪花id"),
                    Method = table.Column<string>(type: "VARCHAR(10)", nullable: true, comment: "请求方法"),
                    Host = table.Column<string>(type: "VARCHAR(200)", nullable: true, comment: "服务主机"),
                    Path = table.Column<string>(type: "TEXT", nullable: true, comment: "请求路径"),
                    QueryString = table.Column<string>(type: "TEXT", nullable: true, comment: "请求url参数"),
                    RequestHeaders = table.Column<string>(type: "TEXT", nullable: true, comment: "请求头"),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: true, comment: "请求消息体"),
                    IpAddress = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "客户端ip"),
                    ConnectionId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "请求连接id"),
                    TraceId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "追踪id"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "Event Id"),
                    EventName = table.Column<string>(type: "VARCHAR(200)", nullable: true, comment: "Event Name"),
                    LogLevel = table.Column<string>(type: "VARCHAR(50)", nullable: false, comment: "日志级别"),
                    Logger = table.Column<string>(type: "VARCHAR(500)", nullable: false, comment: "Logger"),
                    Message = table.Column<string>(type: "TEXT", nullable: false, comment: "日志内容"),
                    ExceptionSource = table.Column<string>(type: "TEXT", nullable: true, comment: "异常源"),
                    ExceptionMessage = table.Column<string>(type: "TEXT", nullable: true, comment: "异常消息"),
                    ExceptionStackTrace = table.Column<string>(type: "TEXT", nullable: true, comment: "异常堆栈"),
                    LogTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "UTC时间"),
                    UserId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "系统登录用户id"),
                    UserName = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "系统登录用户名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HttpLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键，雪花id"),
                    Method = table.Column<string>(type: "VARCHAR(10)", nullable: false, comment: "请求方法"),
                    Host = table.Column<string>(type: "VARCHAR(200)", nullable: false, comment: "服务主机"),
                    Path = table.Column<string>(type: "TEXT", nullable: false, comment: "请求路径"),
                    QueryString = table.Column<string>(type: "TEXT", nullable: true, comment: "请求url参数"),
                    RequestHeaders = table.Column<string>(type: "TEXT", nullable: true, comment: "请求头"),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: true, comment: "请求消息体"),
                    IpAddress = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "客户端ip"),
                    ConnectionId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "请求连接id"),
                    TraceId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "追踪id"),
                    Duration = table.Column<TimeSpan>(type: "TIME", nullable: false, comment: "请求响应持续时间"),
                    StatusCode = table.Column<int>(type: "int", nullable: false, comment: "响应状态码"),
                    ResponseHeaders = table.Column<string>(type: "TEXT", nullable: true, comment: "响应头"),
                    ResponseBody = table.Column<string>(type: "TEXT", nullable: true, comment: "响应消息体"),
                    LogTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "UTC时间"),
                    UserId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "系统登录用户id"),
                    UserName = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "系统登录用户名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpLogs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLogs");

            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "HttpLogs");
        }
    }
}
