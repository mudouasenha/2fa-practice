using Doodle.Domain.Entities;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Doodle.Services.Users.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUsersService _usersService;

        public UserController(ILogger<UserController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet()]
        public async Task<IEnumerable<User>> Get()
        {
            return new List<User> { new User() };
        }

        [HttpPost("register")]
        public async Task<User> Register()
        {
            return new User();
        }

        [HttpPost("signin")]
        public async Task<User> SignIn()
        {
            return new User();
        }

        [HttpDelete("deleteaccount")]
        public async Task<bool> DeleteAccount()
        {
            return true;
        }
    }
}