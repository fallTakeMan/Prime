using Extensions.Logging.Prime.Model;
using Extensions.Logging.Prime.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Web;

namespace Extensions.Logging.Prime
{
    internal class HostedEndpointRouteProvider
    {
        private readonly IEndpointRouteBuilder _endpoint;
        private readonly IServiceProvider _serviceProvider;
        private readonly PrimeOptions _options;

        public HostedEndpointRouteProvider(IEndpointRouteBuilder endpoint, PrimeOptions options)
        {
            _endpoint = endpoint;
            _options = options;
            _serviceProvider = endpoint.ServiceProvider;
        }

        public void MapApiRoute()
        {
            _endpoint.MapPost(_options.RouteMatch + "/api/Account/SignIn", SignIn).AllowAnonymous();
            _endpoint.MapGet(_options.RouteMatch + "/api/dashboard/stats", GetTodayStatsAsync).RequireAuthorization(PrimeAuthorizationPolicyDefaults.Policy);
            _endpoint.MapGet(_options.RouteMatch + "/api/logs/app", GetAppLogsAsync).RequireAuthorization(PrimeAuthorizationPolicyDefaults.Policy);
            _endpoint.MapDelete(_options.RouteMatch + "/api/logs/app", DeleteAppLogsAsync).RequireAuthorization(PrimeAuthorizationPolicyDefaults.Policy);
            _endpoint.MapGet(_options.RouteMatch + "/api/logs/http", GetHttpLogsAsync).RequireAuthorization(PrimeAuthorizationPolicyDefaults.Policy);
            _endpoint.MapDelete(_options.RouteMatch + "/api/logs/http", DeleteHttpLogsAsync).RequireAuthorization(PrimeAuthorizationPolicyDefaults.Policy);
            _endpoint.MapGet(_options.RouteMatch + "/api/logs/exception", GetExceptionLogsAsync).RequireAuthorization(PrimeAuthorizationPolicyDefaults.Policy);
            _endpoint.MapDelete(_options.RouteMatch + "/api/logs/exception", DeleteExceptionLogsAsync).RequireAuthorization(PrimeAuthorizationPolicyDefaults.Policy);
        }

        private async Task SignIn(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                var bodyReadTask = await context.Request.BodyReader.ReadAsync();
                var reqStr = Encoding.UTF8.GetString(bodyReadTask.Buffer);
                context.Request.Body.Position = 0;
                var req = System.Text.Json.JsonSerializer.Deserialize<PrimeSignIn>(reqStr);
                if (req.UserName.Equals(PrimeOptionsStatic.UserName, StringComparison.CurrentCultureIgnoreCase) &&
                    req.Password.Equals(PrimeOptionsStatic.Password, StringComparison.CurrentCultureIgnoreCase))
                {
                    var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{req.UserName}:{req.Password}"));
                    
                    await context.Response.WriteAsync(basic);
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(PrimeApiResult.Error(e.Message));
            }
        }

        private async Task GetTodayStatsAsync(HttpContext context)
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                var data = await service.GetTodayStatsAsync();
                await context.Response.WriteAsJsonAsync(data);
            }
        }

        private async Task GetAppLogsAsync(HttpContext context)
        {
            var param = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
            int.TryParse(param.Get("index") ?? "1", out int index);
            int.TryParse(param.Get("size") ?? "20", out int size);
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                var data = await service.GetAppLogsAsync(index, size);
                await context.Response.WriteAsJsonAsync(data);
            }
        }

        private async Task DeleteAppLogsAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                var bodyReadTask = await context.Request.BodyReader.ReadAsync();
                var reqStr = Encoding.UTF8.GetString(bodyReadTask.Buffer);
                context.Request.Body.Position = 0;
                var req = System.Text.Json.JsonSerializer.Deserialize<IdDeletion>(reqStr);
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                    var result = await service.DeleteAppLogsAsync(req.Ids);
                }
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(PrimeApiResult.Error(e.Message));
            }
        }

        private async Task GetHttpLogsAsync(HttpContext context)
        {
            var param = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
            int.TryParse(param.Get("index") ?? "1", out int index);
            int.TryParse(param.Get("size") ?? "20", out int size);
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                var data = await service.GetHttpLogsAsync(index, size);
                await context.Response.WriteAsJsonAsync(data);
            }
        }

        private async Task DeleteHttpLogsAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                var bodyReadTask = await context.Request.BodyReader.ReadAsync();
                var reqStr = Encoding.UTF8.GetString(bodyReadTask.Buffer);
                context.Request.Body.Position = 0;
                var req = System.Text.Json.JsonSerializer.Deserialize<IdDeletion>(reqStr);
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                    var result = await service.DeleteHttpLogsAsync(req.Ids);
                    await context.Response.WriteAsJsonAsync(PrimeApiResult.Success());
                }
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(PrimeApiResult.Error(e.Message));
            }
        }

        private async Task GetExceptionLogsAsync(HttpContext context)
        {
            var param = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
            int.TryParse(param.Get("index") ?? "1", out int index);
            int.TryParse(param.Get("size") ?? "20", out int size);
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                var data = await service.GetExceptionLogsAsync(index, size);
                await context.Response.WriteAsJsonAsync(data);
            }
        }

        private async Task DeleteExceptionLogsAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                var bodyReadTask = await context.Request.BodyReader.ReadAsync();
                var reqStr = Encoding.UTF8.GetString(bodyReadTask.Buffer);
                context.Request.Body.Position = 0;
                var req = System.Text.Json.JsonSerializer.Deserialize<IdDeletion>(reqStr);
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                    var result = await service.DeleteExceptionLogsAsync(req.Ids);
                    await context.Response.WriteAsJsonAsync(PrimeApiResult.Success());
                }
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(PrimeApiResult.Error(e.Message));
            }
        }
    }
}
