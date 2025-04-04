using Extensions.Logging.Prime.Model;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Extensions.Logging.Prime.Service
{
    internal class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public HttpContextService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public LoggerUser GetUser()
        {
            var uid = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var uname = _contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            return new LoggerUser { UserId = uid?.Value, UserName = uname?.Value ?? string.Empty };
        }

        public HttpContext Context => _contextAccessor.HttpContext;

    }

}
