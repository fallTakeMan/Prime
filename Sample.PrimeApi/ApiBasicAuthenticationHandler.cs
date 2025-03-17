using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Sample.PrimeApi
{
    public class ApiAuthorizationPolicyDefaults
    {
        public const string Policy = "ApiPolicy";
    }
    public class ApiAuthenticationSchemeDefaults
    {
        public const string Scheme = "ApiAuthenticationScheme";
    }
    public class ApiAuthenticationSchemeOptions : AuthenticationSchemeOptions { }

    public class ApiBasicAuthenticationHandler : AuthenticationHandler<ApiAuthenticationSchemeOptions>
    {
        private readonly ApiDbContext _db;
        public ApiBasicAuthenticationHandler(IOptionsMonitor<ApiAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ApiDbContext db) 
            : base(options, logger, encoder)
        {
            _db = db;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }
            var basicAuth = Request.Headers.Authorization.Any(x => x.StartsWith("Basic"));
            if (basicAuth)
            {
                if (System.Net.Http.Headers.AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var base64Encoded))
                {
                    var base64bytes = Convert.FromBase64String(base64Encoded.Parameter);
                    var info = Encoding.UTF8.GetString(base64bytes).Split(':', 2);
                    var user = _db.Users.FirstOrDefault(x => x.Name == info[0] && x.Password == info[1]);
                    if (user != null)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, user.Name), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
                        var identity = new ClaimsIdentity(claims, ApiAuthenticationSchemeDefaults.Scheme);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, ApiAuthenticationSchemeDefaults.Scheme);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                    else
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Account or password error."));
                    }
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authorization Header is not in the correct format."));
                }
            }
            else
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
        }
    }
}
