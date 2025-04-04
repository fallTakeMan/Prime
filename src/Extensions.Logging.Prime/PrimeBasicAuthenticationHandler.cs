using Extensions.Logging.Prime.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Extensions.Logging.Prime
{
    internal class PrimeAuthorizationPolicyDefaults
    {
        public const string Policy = "PrimePolicy";
    }
    internal class PrimeAuthenticationSchemeDefaults
    {
        public const string Scheme = "PrimeAuthenticationScheme";
    }
    internal class PrimeAuthenticationSchemeOptions : AuthenticationSchemeOptions { }
    internal class PrimeBasicAuthenticationHandler : AuthenticationHandler<PrimeAuthenticationSchemeOptions>
    {
        public PrimeBasicAuthenticationHandler(IOptionsMonitor<PrimeAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
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
                    if (info[0].Equals(PrimeOptionsStatic.UserName, StringComparison.CurrentCultureIgnoreCase) &&
                        info[1].Equals(PrimeOptionsStatic.Password, StringComparison.CurrentCultureIgnoreCase))
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, info[0]) };
                        var identity = new ClaimsIdentity(claims, PrimeAuthenticationSchemeDefaults.Scheme);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, PrimeAuthenticationSchemeDefaults.Scheme);
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
