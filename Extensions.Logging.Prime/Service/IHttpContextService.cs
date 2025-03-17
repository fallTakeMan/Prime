using Extensions.Logging.Prime.Model;
using Microsoft.AspNetCore.Http;

namespace Extensions.Logging.Prime.Service
{
    internal interface IHttpContextService
    {
        LoggerUser GetUser();

        HttpContext Context { get; }
    }
}
