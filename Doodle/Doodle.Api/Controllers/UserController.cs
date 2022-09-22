using Doodle.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<User> Get()
        {
            return new List<User> { new User() };
        }

        [HttpPost("signup")]
        public User SignUp()
        {
            return new User();
        }

        [HttpPost("signin")]
        public User SignIn()
        {
            return new User();
        }

        [HttpDelete("deleteaccount")]
        public bool DeleteAccount()
        {
            return true;
        }
    }
}