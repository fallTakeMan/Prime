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

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
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
        public IActionResult Hello(string name)
        {
            return Ok($"Hello, {name}");
        }
    }
}
