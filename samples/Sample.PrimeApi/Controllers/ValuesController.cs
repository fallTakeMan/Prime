using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.PrimeApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly ApiDbContext _db;

        public ValuesController(ILogger<ValuesController> logger, ApiDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult UnhandledException()
        {
            var a = 1;
            var b = 0;
            var result = a / b;
            return Ok(0);
        }

        [HttpGet]
        public IActionResult TryException()
        {
            try
            {
                var a = 1;
                var b = 0;
                var result = a / b;
                return Ok(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateUserRole(UserRoleModel req)
        {
            var e = _db.Users.Add(new TestUser 
            { 
                Name = req.UserName,
                Password = req.Password
            });
            _db.Roles.Add(new TestRole { RoleName = req.RoleName, Description = req.RoleDescription });
            _db.SaveChanges();
            return Accepted(e.Entity.Id);
        }

        [HttpPut]
        public IActionResult ChangePassword(ModifyUserPasswordModel req)
        {
            var user = _db.Users.Find(req.Id);
            if (user == null)
            {
                return NotFound();
            }
            _db.Attach(user);
            user.Password = req.Password;
            _db.Entry(user).Property(p => p.Password).IsModified = true;
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _db.Users.Remove(user);
            _db.SaveChanges();
            return Ok();
        }
    }

    public class UserRoleModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        
        public required string RoleName { get; set; }
        public string? RoleDescription { get; set; }
    }

    public class ModifyUserPasswordModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public required string Password { get; set; }
    }
}
