using Extensions.Logging.Prime.Database;
using Extensions.Logging.Prime.Database.Configuration;
using Extensions.Logging.Prime.Database.Entity;
using Extensions.Logging.Prime.Service;
using Extensions.Logging.Prime.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Extensions.Logging.Prime
{
    internal class PrimeHttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public PrimeHttpLoggingMiddleware(RequestDelegate next, IServiceProvider provider)
        {
            _next = next;
            _serviceProvider = provider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            if (IsMatch(path))
            {
                await _next(context);
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

                var log = new HttpLogEntity
                {
                    Id = IdWorker.SnowflakeId(),
                    Method = context.Request.Method,
                    Host = context.Request.Host.Value ?? "",
                    Path = path,
                    QueryString = context.Request.QueryString.Value ?? string.Empty,
                    RequestHeaders = context.Request.Headers.Any() ? context.Request.Headers.Select(x => x.ToString()).Aggregate((a, b) => a + ":" + b) : string.Empty,
                    IpAddress = GetClientIp(context),
                    ConnectionId = context.Connection.Id,
                    TraceId = context.TraceIdentifier,
                    LogTime = DateTime.UtcNow
                };

                context.Request.EnableBuffering();
                var reqBody = context.Request.BodyReader.ReadAsync();
                if (reqBody.IsCompleted)
                {
                    log.RequestBody = Encoding.UTF8.GetString(reqBody.Result.Buffer);
                }
                context.Request.Body.Position = 0;

                var original = context.Response.Body;
                using var ms = new MemoryStream();
                context.Response.Body = ms;

                await _next(context);

                ms.Position = 0;
                var sr = new StreamReader(ms, Encoding.UTF8);
                var resp = await sr.ReadToEndAsync();
                ms.Position = 0;
                await ms.CopyToAsync(original);
                context.Response.Body = original;

                log.Duration = DateTime.UtcNow.Subtract(log.LogTime);
                log.StatusCode = context.Response.StatusCode;
                log.ResponseBody = resp;
                log.ResponseHeaders = context.Response.Headers.Any() ? context.Response.Headers.Select(x => x.ToString()).Aggregate((a, b) => a + ":" + b) : string.Empty;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var ctxService = _serviceProvider.GetRequiredService<IHttpContextService>();
                    var user = ctxService.GetUser();
                    log.UserId = user?.UserId ?? string.Empty;
                    log.UserName = user?.UserName ?? string.Empty;

                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();
                    db.HttpLogs.Add(log);

                    await db.SaveChangesAsync();

                }
            }
        }

        private bool IsMatch(string path)
        {
            string[] mime = [".aac", ".abw", ".apng", ".arc", ".avif", ".avi", ".azw", ".bin", ".bmp", ".bz", ".bz2", ".cda", ".csh", ".css", ".csv", ".doc", ".docx", ".eot", ".epub", ".gz", ".gif", ".htm", ".html", ".ico", ".ics", ".jar", ".jpeg", ".jpg", ".js", ".json", ".jsonld", ".mid", ".midi", ".mjs", ".mp3", ".mp4", ".mpeg", ".mpkg", ".odp", ".ods", ".odt", ".oga", ".ogv", ".ogx", ".opus", ".otf", ".png", ".pdf", ".php", ".ppt", ".pptx", ".rar", ".rtf", ".sh", ".svg", ".tar", ".tif", ".tiff", ".ts", ".ttf", ".txt", ".vsd", ".wav", ".weba", ".webm", ".webp", ".woff", ".woff2", ".xhtml", ".xls", ".xlsx", ".xml", ".xul", ".zip", ".3gp", ".3g2", ".7z"];

            return mime.Any(x => path.EndsWith(x, StringComparison.CurrentCultureIgnoreCase)) ||
                path.Equals("/") ||
                path.StartsWith(PrimeOptionsConfig.RouteMatch, StringComparison.CurrentCultureIgnoreCase) ||
                PrimeOptionsConfig.HttpLogIgnoreRouteMatch.Split(',').Where(x => x.Trim().Length > 0).Any(x => path.StartsWith(x, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
