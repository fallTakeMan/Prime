using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Extensions.Logging.Prime
{
    internal sealed class PrimeStartup : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<PrimeHttpLoggingMiddleware>(app.ApplicationServices);

                next(app);

                app.UsePrime();
            };
        }
    }
}
