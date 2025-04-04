using Extensions.Logging.Prime.Database;
using Extensions.Logging.Prime.Model;
using Extensions.Logging.Prime.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Text;

namespace Extensions.Logging.Prime
{
    public static class PrimeExtensions
    {
        private static readonly string _distPath = "Extensions.Logging.Prime.wwwroot.dist";
        public static ILoggingBuilder AddPrime(this ILoggingBuilder builder, [Optional] Action<PrimeOptions> func)
        {
            var prime = new PrimeOptions();
            func?.Invoke(prime);
            if (string.IsNullOrWhiteSpace(prime.ConnectionString))
            {
                throw new ArgumentNullException(nameof(prime.ConnectionString));
            }
            builder.Services.TryAddSingleton(prime);

            builder.Services.AddDbContext<LogDbContext>(opt => _ = prime.DbProvider switch
            {
                DatabaseType.MSSQL => opt.UseSqlServer(prime.ConnectionString, x => x.MigrationsAssembly(typeof(Extensions.Logging.Prime.Migrations.SqlServer.IPrimeMigration).Assembly.GetName().Name!)),
                DatabaseType.MySql => opt.UseMySQL(prime.ConnectionString, x => x.MigrationsAssembly(typeof(Extensions.Logging.Prime.Migrations.MySql.IPrimeMigration).Assembly.GetName().Name!)),
                DatabaseType.PostgreSQL => opt.UseNpgsql(prime.ConnectionString, x => x.MigrationsAssembly(typeof(Extensions.Logging.Prime.Migrations.PostgreSQL.IPrimeMigration).Assembly.GetName().Name!)),
                _ => throw new NotSupportedException(nameof(prime.DbProvider))
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.TryAddTransient<IHttpContextService, HttpContextService>();
            builder.Services.TryAddTransient<ILogService, LogService>();
            builder.Services.AddTransient<IStartupFilter, PrimeStartup>();

            builder.Services.AddAuthentication().AddScheme<PrimeAuthenticationSchemeOptions, PrimeBasicAuthenticationHandler>(PrimeAuthenticationSchemeDefaults.Scheme, null);
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(PrimeAuthorizationPolicyDefaults.Policy, policy =>
                policy.AddAuthenticationSchemes(PrimeAuthenticationSchemeDefaults.Scheme).RequireAuthenticatedUser());
            });

            var provider = builder.Services.BuildServiceProvider();
            builder.AddProvider(new PrimeLoggerProvider(provider));

            PrimeOptionsStatic.RouteMatch = prime.RouteMatch;
            PrimeOptionsStatic.HttpLogIgnoreRouteMatch = prime.HttpLogIgnoreRouteMatch;
            PrimeOptionsStatic.UserName = prime.PrimeUserName;
            PrimeOptionsStatic.Password = prime.PrimePassword;

            return builder;
        }

        public static IApplicationBuilder UsePrime(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            var opt = app.ApplicationServices.GetService<PrimeOptions>();
            if (opt != null)
            {
                ConfigureStaticFiles(app, opt);
                ConfigureEndpoints(app, opt);
            }
            return app;
        }

        private static void ConfigureStaticFiles(IApplicationBuilder app, PrimeOptions opt)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = opt.RouteMatch,
                FileProvider = new EmbeddedFileProvider(opt.GetType().Assembly, _distPath),
            });
        }

        private static void ConfigureEndpoints(IApplicationBuilder app, PrimeOptions opt)
        {
            var endpoint = (IEndpointRouteBuilder)app.Properties["__EndpointRouteBuilder"]!;
            endpoint.MapGet(opt.RouteMatch, httpContext =>
            {
                var path = httpContext.Request.Path.Value;
                var redirectUrl = string.IsNullOrWhiteSpace(path) || path.EndsWith("/") ? "index.html" : $"{path.Split('/').Last()}/index.html";
                httpContext.Response.StatusCode = 301;
                httpContext.Response.Headers["Location"] = redirectUrl;
                return Task.CompletedTask;
            });

            endpoint.MapGet(opt.RouteMatch + "/index.html", async httpContext =>
            {
                httpContext.Response.StatusCode = 200;
                httpContext.Response.ContentType = "text/html;charset=utf-8";
                await using var stream = opt.GetType().Assembly.GetManifestResourceStream(_distPath + ".index.html");
                if (stream == null) throw new InvalidOperationException();
                using var reader = new StreamReader(stream);
                var html = new StringBuilder(await reader.ReadToEndAsync());
                html.Replace("%(hosted_api)%", opt.RouteMatch + "/api");
                await httpContext.Response.WriteAsync(html.ToString(), Encoding.UTF8);
            });

            new HostedEndpointRouteProvider(endpoint, opt).MapApiRoute();
        }

    }
}
