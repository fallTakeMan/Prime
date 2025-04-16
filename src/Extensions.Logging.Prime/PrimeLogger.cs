using Extensions.Logging.Prime.Database;
using Extensions.Logging.Prime.Database.Configuration;
using Extensions.Logging.Prime.Database.Entity;
using Extensions.Logging.Prime.Service;
using Extensions.Logging.Prime.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Extensions.Logging.Prime
{
    internal class PrimeLogger : ILogger
    {
        private readonly string _name;
        private readonly IServiceProvider _serviceProvider;

        public PrimeLogger(string name, IServiceProvider serviceProvider)
        {
            _name = name;
            _serviceProvider = serviceProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel level) => level != LogLevel.None;

        public async void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formater)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var ctxService = _serviceProvider.GetRequiredService<IHttpContextService>();
            var user = ctxService?.GetUser()!;
            if (string.IsNullOrWhiteSpace(user.UserId) && user.UserName.Equals(PrimeOptionsConfig.UserName, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }
            if (exception is null)
            {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();
                    var log = new AppLogEntity
                    {
                        Id = IdWorker.SnowflakeId(),
                        EventId = eventId.Id,
                        EventName = eventId.Name ?? string.Empty,
                        LogLevel = logLevel.ToString(),
                        Logger = _name,
                        Message = formater(state, exception),
                        LogTime = DateTime.UtcNow,
                        UserId = user?.UserId ?? "",
                        UserName = user?.UserName ?? ""
                    };

                    db.AppLogs.Add(log);

                    await db.SaveChangesAsync();
                }
            }
            else
            {
                string GetClientIp(HttpContext ctx)
                {
                    var x = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(x))
                    {
                        return x.Split(',').FirstOrDefault() ?? string.Empty;
                    }
                    return ctx.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                }

                var ctx = ctxService?.Context;

                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();
                    var log = new ExceptionLogEntity
                    {
                        Id = IdWorker.SnowflakeId(),
                        Method = ctx.Request.Method ?? string.Empty,
                        Host = ctx.Request.Host.Value ?? string.Empty,
                        Path = ctx.Request.Path.Value ?? string.Empty,
                        QueryString = ctx.Request.QueryString.Value ?? string.Empty,
                        RequestHeaders = ctx.Request.Headers.Any() ? ctx.Request.Headers.Select(x => x.ToString()).Aggregate((a, b) => a + ":" + b) : string.Empty,
                        RequestBody = "",
                        IpAddress = GetClientIp(ctx),
                        ConnectionId = ctx?.Connection?.Id ?? string.Empty,
                        TraceId = ctx?.TraceIdentifier ?? string.Empty,
                        EventId = eventId.Id,
                        EventName = eventId.Name ?? string.Empty,
                        LogLevel = logLevel.ToString(),
                        Logger = _name,
                        Message = formater(state, exception),
                        ExceptionSource = exception.Source,
                        ExceptionMessage = exception.Message,
                        ExceptionStackTrace = exception.StackTrace,
                        LogTime = DateTime.UtcNow,
                        UserId = user?.UserId ?? string.Empty,
                        UserName = user?.UserName ?? string.Empty
                    };
                    ctx.Request.EnableBuffering();
                    var reqBody = ctx.Request.BodyReader.ReadAsync();
                    if (reqBody.IsCompleted)
                    {
                        log.RequestBody = Encoding.UTF8.GetString(reqBody.Result.Buffer);
                    }
                    ctx.Request.Body.Position = 0;
                    db.ExceptionLogs.Add(log);
                    await db.SaveChangesAsync();
                }
            }

        }
    }
}
