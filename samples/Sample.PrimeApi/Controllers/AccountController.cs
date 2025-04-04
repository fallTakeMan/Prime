using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Sample.PrimeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger _logger;
        private readonly ApiDbContext _db;
        public AccountController(IHttpContextAccessor contextAccessor, ILogger<AccountController> logger, ApiDbContext db)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _db = db;
        }

        [HttpPost]
        public IActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _db.Users.FirstOrDefault(x => x.Name == model.UserName && x.Password == model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));

            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
            };

            _contextAccessor.HttpContext?.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.Name}:{user.Password}"));
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public IActionResult LogOut()
        {
            var user = _contextAccessor.HttpContext.User.Identity.Name;
            _logger.LogInformation($"{user} sign out");
            _contextAccessor.HttpContext?.SignOutAsync();
            return Ok();
        }
    }

    public class SignInModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
